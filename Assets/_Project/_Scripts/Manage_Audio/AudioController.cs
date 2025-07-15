using System;
using System.Collections;          
using UnityEngine;
using CF.Data;

namespace CF.Audio {
public class AudioController : MonoBehaviour
{
    // members
    public static AudioController Current = null;

    public bool debug;

    [Tooltip("Default Volume that will be used for any track")]
    public float m_defaultVolume;

    [Tooltip("Select the type and the corresponding audio clip")]
    public AudioTrack[] tracks;

    private Hashtable m_AudioTable; // relationship between audio types (key) and audio tracks (value)
    private Hashtable m_JobTable; // relationship between audio types (key) and jobs (value) (Coroutine)
    private Hashtable m_VolumeTable; // relationship between audio types (key) and volume values (value) (float)
    private Hashtable m_TrackTypeTable; // relationship between Track Type (key) and audio tracks ()

    [System.Serializable]
    public class AudioObject
    {
        public AudioOfType type;
        public AudioClip clip;
    }
    [System.Serializable]
    public class AudioTrack
    {
        [Tooltip("The Reference the Volume Settings will use to find this track")]
        public AudioTrackType TrackType;
        public AudioSource source;
        public AudioObject[] audio;
        public float Volume 
        {
            get { return source.volume; }
            set 
            {
                var _volume = Mathf.Clamp01(value);
                source.volume = _volume;
            }
        }
        
    }
    private class AudioJob
    {
        public AudioAction action;
        public AudioOfType type;
        public bool fade;
        public float delay;

        public AudioJob(AudioAction _action, AudioOfType _type, bool _fade, float _delay)
        {
            action = _action;
            type = _type;
            fade = _fade;
            delay = _delay;
        }
    }

    private enum AudioAction
    {
        START,
        STOP,
        RESTART,
        ONTOP
    }


    #region Unity Functions
    private void Awake()
    {
        if (!Current)
        {
            Configure();
        }
    }

    private void OnDisable()
    {
        Dispose();
    }
    #endregion

    #region Public Functions
    public void PlayAudio(AudioOfType _type, bool _fade = false, float _delay = 0.0f)
    {
        AddJob(new AudioJob(AudioAction.START, _type, _fade, _delay));
    }
    public void PlayAudioOnTop(AudioOfType _type, bool _fade = false, float _delay = 0.0f){
            AddJob(new AudioJob(AudioAction.ONTOP, _type, _fade, _delay));
    }
    public void StopAudio(AudioOfType _type, bool _fade = false, float _delay = 0.0f)
    {
        AddJob(new AudioJob(AudioAction.STOP, _type, _fade, _delay));
    }
    public void RestartAudio(AudioOfType _type, bool _fade = false, float _delay = 0.0f)
    {
        AddJob(new AudioJob(AudioAction.RESTART, _type, _fade, _delay));
    }
    public void SetTrackVolume(AudioTrackType _type, float _volume) 
    {
        AdjustVolumeAtTrack(_type, _volume);
    }
    
    public void SaveVolumePreferences()
    {
        DataController.SaveVolumeData(m_VolumeTable);
    }
    public void LoadVolumePreferences()
    {
        m_VolumeTable = DataController.LoadVolumeData();
        UpdateAllTrackVolumes();
    }

    #endregion

    #region Private Functions
    private void Configure()
    {
        Current = this;
        m_AudioTable = new Hashtable();
        m_JobTable = new Hashtable();
        m_VolumeTable = new Hashtable();
        m_TrackTypeTable = new Hashtable();
        GenerateAudioTable();
        GenerateTrackTypeTable();
        GenerateVolumeTable();
        LoadVolumePreferences();
    }

    private void Dispose()
    {
        foreach (DictionaryEntry _entry in m_JobTable)
        {
            IEnumerator _job = (IEnumerator)_entry.Value;
            StopCoroutine(_job);
        }
    }

    private void GenerateAudioTable()
    {
        foreach (AudioTrack _track in tracks)
        {
            foreach (AudioObject _obj in _track.audio)
            {
                // do not duplicate keys
                if (m_AudioTable.ContainsKey(_obj.type))
                {
                    LogWarning("You are tring to register audio [" + _obj.type + "] that has already been registered.");
                }
                else
                {
                    m_AudioTable.Add(_obj.type, _track);
                    Log("Registering audio [" + _obj.type + "].");
                }
            }
        }
    }

    #region Job Functions
    private IEnumerator RunAudioJob(AudioJob _job)
    {
        yield return new WaitForSeconds(_job.delay);

        AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
        _track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);

        switch (_job.action)
        {
            case AudioAction.START:
                _track.source.Play();
                break;
            case AudioAction.ONTOP:
                _track.source.PlayOneShot(_track.source.clip);
                break;
            case AudioAction.STOP:
                if (!_job.fade)
                {
                    _track.source.Stop();
                }
                break;
            case AudioAction.RESTART:
                _track.source.Stop();
                _track.source.Play();
                break;
        }

        if (_job.fade)
        {
            float _initial = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 0.0f : 1.0f; // TODO Sets to full volume
            float _target = _initial == 0 ? 1 : 0;
            float _duration = 1.0f; // Hard coded fade
            float _timer = 0.0f;

            while (_timer <= _duration)
            {
                _track.source.volume = Mathf.Lerp(_initial, _target, _timer / _duration);
                _timer += Time.deltaTime;
                yield return null;
            }

            if (_job.action == AudioAction.STOP)
            {
                _track.source.Stop();
            }
        }

        m_JobTable.Remove(_job.type);
        Log("Job count: " + m_JobTable.Count.ToString());

        yield return null;
    }

    private void AddJob(AudioJob _job)
    {
        // remove conflicting jobs
        RemoveConflictingJobs(_job.type);

        // start job
        IEnumerator _jobRunner = RunAudioJob(_job);
        m_JobTable.Add(_job.type, _jobRunner);
        StartCoroutine(_jobRunner);
        Log("Starting job on[" + _job.type + "] with operation" + _job.action);
    }

    private void RemoveJob(AudioOfType _type)
    {
        if (!m_JobTable.ContainsKey(_type))
        {
            LogWarning("Trying to stop a job [" + _type + "] that is not running.");
            return;
        }

        IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
        StopCoroutine(_runningJob);
        m_JobTable.Remove(_type);
    }

    private void RemoveConflictingJobs(AudioOfType _type)
    {
        if (m_JobTable.ContainsKey(_type))
        {
            RemoveJob(_type);
        }

        AudioOfType _conflictAudio = AudioOfType.None;
        AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_type];
        foreach (DictionaryEntry _entry in m_JobTable)
        {
            AudioOfType _audioType = (AudioOfType)_entry.Key;
            AudioTrack _audioTrackInUse = (AudioTrack)m_AudioTable[_audioType];
            if (_audioTrackNeeded.source == _audioTrackInUse.source)
            {
                // conflict
                _conflictAudio = _audioType;
            }
        }
        if (_conflictAudio != AudioOfType.None)
        {
            RemoveConflictingJobs(_conflictAudio);
        }
    }
    #endregion


    #region Volume Functions

    private void GenerateVolumeTable() 
    {
        /*
        var values = Enum.GetValues(typeof(AudioTrackType));
        foreach (AudioTrackType _type in values)
        {
            m_VolumeTable.Add(_type, m_defaultVolume);
            
        }
        */
        foreach (var t in tracks)
        {
            m_VolumeTable.Add(t.TrackType, t.Volume);
        }
    }

    private void GenerateTrackTypeTable() 
    {
        var values = Enum.GetValues(typeof(AudioTrackType));
        foreach (AudioTrack _track in tracks)
        {
            m_TrackTypeTable.Add(_track.TrackType, _track);
            Log("Registered [" + _track.source.name + "] Track with type [" + _track.TrackType + "].");
        }
        
    }

    private void AdjustVolumeAtTrack(AudioTrackType _type, float _volume)
    {
        m_VolumeTable[_type] = _volume;
        UpdateTrackVolume(_type);
    }


    private void UpdateAllTrackVolumes() 
    {
        foreach (DictionaryEntry _pair in m_TrackTypeTable)
        {
            AudioTrack _source = (AudioTrack) _pair.Value;
            _source.Volume = (float) m_VolumeTable[_pair.Key];
        }
    }

    private void UpdateTrackVolume(AudioTrackType _type) 
    {
        if (!m_TrackTypeTable.ContainsKey(_type))
        {
            LogWarning("You're trying to Update the Volume of [" + _type + "] that isn't in use.");
            return;
        }

        float _volume = (float)m_VolumeTable[_type]; // Get the Volume the Track should use

        var _track = (AudioTrack)m_TrackTypeTable[_type];
        _track.Volume = _volume; // Change the Volume Property of track
    }

    
    #endregion

    private AudioClip GetAudioClipFromAudioTrack(AudioOfType _type, AudioTrack _track)
    {
        foreach (AudioObject _obj in _track.audio)
        {
            if (_obj.type == _type)
            {
                return _obj.clip;
            }
        }
        return null;
    }

    private void Log(string _msg)
    {
        if (!debug) return;
        Debug.Log("[Audio Controller]: " + _msg);
    }
    private void LogWarning(string _msg)
    {
        if (!debug) return;
        Debug.LogWarning("[Audio Controller]: " + _msg);
    }


    #endregion
}
}