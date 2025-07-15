using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System;
using System.Collections.Generic;
using CF.Player;
using CF.Audio;

namespace CF.Data {

public static class DataController
{
    /* Advanced Data Controller that can save complicated game states
        * Manages [Inventorys, Player Progress, other abstract cases] */

    #region Fields
    private static readonly string m_PathEnding = ".save";
    private static readonly string m_VolumeFile = "volume_settings";

    private static readonly string m_ShipResourcePath = "PlayerShips/";
    private static readonly string m_ShipsOwned = "/ships_owned";
    private static readonly string m_ShipEquiped = "/ship";

    private static readonly string m_AttacksResourcePath = "PlayerAttacks/";
    private static readonly string m_AttacksOwned = "/attacks_owned";
    private static readonly string m_AttackEquiped = "/attack";

    private static readonly string m_SpecialsResourcePath = "PlayerSpecials/";
    private static readonly string m_SpecialsOwned = "/specials_owned";
    private static readonly string m_SpecialEquiped = "/special";

    private static readonly string m_ShieldsResourcePath = "PlayerShields/";
    private static readonly string m_ShieldsOwned = "/shields_owned";
    private static readonly string m_ShieldEquiped = "/shield";

    private static readonly string m_DefaultShip = "cova_ship";
    private static readonly string m_DefaultAttack = "rusty_cannons_attack";
    private static readonly string m_DefaultShield = "default_shield";
    private static readonly string m_DefaultSpecial = "default_special";

    #endregion

    #region Save Functions

    #region Generalized String Data Structure
    public static List<string> LoadStringListData(string data_path, string default_value)
    {
        string path = Application.persistentDataPath + data_path + m_PathEnding;
        if (File.Exists(path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(path, FileMode.Open);

            List<string> data = (List<string>)_formatter.Deserialize(_stream);

            _stream.Close();

            //Log($"Loaded {data_path}", "Data Controller");

            return data;
        }
        else
        {
            LogWarning("Save file not found in " + path, "Data Controller");

            List<string> default_data = new List<string>();

            default_data.Add(default_value);

            AddStringListData(default_value, data_path, new List<string>());

            return default_data;
        }
    }

    public static void AddStringListData(string resource_name,string data_path, List<string> data_list )
    {
        data_list.Add(resource_name);

        BinaryFormatter _formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + data_path + m_PathEnding;
        FileStream stream = new FileStream(path, FileMode.Create);

        _formatter.Serialize(stream, data_list);

        stream.Close();
    }

    #endregion

    #region Save Specials Owned

    public static void AddPlayerSpecial(string name)
    {
        AddStringListData(name, m_SpecialsOwned, LoadPlayerSpecialsOwned());
    }

    public static List<string> LoadPlayerSpecialsOwned()
    {
        return LoadStringListData(m_SpecialsOwned, m_DefaultSpecial);
    }

    #endregion

    #region Save Attacks Owned

    public static void AddPlayerAttack(string attackName)
    {
        AddStringListData(attackName, m_AttacksOwned, LoadPlayerAttacksOwned());
    }

    public static List<string> LoadPlayerAttacksOwned()
    {
        return LoadStringListData(m_AttacksOwned, m_DefaultAttack);
    }

    #endregion

    #region Save Ships Owned
    // Saves Ship Data By Ressource Name
    public static void AddShip(string resource_name)
    {
        AddStringListData(resource_name, m_ShipsOwned, LoadOwnedShips());
    }

    public static bool IsShipOwned(string resource_name)
    {
        return LoadOwnedShips().Contains(resource_name);
    }

    public static void ResetAllOwnedShips()
    {
        // I don't know a game use case. Use this for debugging. Please.

        List<string> ships_owned = new List<string>();

        ships_owned.Add(m_DefaultShip);

        BinaryFormatter _formatter = new BinaryFormatter();
        string ship_path = Application.persistentDataPath + m_ShipsOwned + m_PathEnding;
        FileStream ship_stream = new FileStream(ship_path, FileMode.Create);

        _formatter.Serialize(ship_stream, ships_owned);

        ship_stream.Close();

    }

    public static List<string> LoadOwnedShips()
    {
        return LoadStringListData(m_ShipsOwned, m_DefaultShip);
    }

    #endregion

    #region Save Shields Owned

    public static void AddPlayerShield(string shieldName)
    {
        AddStringListData(shieldName, m_ShieldsOwned, LoadPlayerShieldsOwned());
    }

    public static List<string> LoadPlayerShieldsOwned()
    {
        return LoadStringListData(m_ShieldsOwned, m_DefaultShield);
    }

    #endregion

    #region Currency Data
    // Saves Currencies

    public static void SaveCurrency(int _shards, int f1=0, int f2 = 0, int f3 = 0, int f4 = 0, int f5 = 0)
    {
        BinaryFormatter _formatter = new BinaryFormatter();
        string _shards_path = Application.persistentDataPath + "/currency" + m_PathEnding;
        FileStream _shards_stream = new FileStream(_shards_path, FileMode.Create);

        CurrencyToken token = new CurrencyToken(_shards, f1, f2, f3, f4, f5);

        _formatter.Serialize(_shards_stream, token);
        _shards_stream.Close();

        Log("Saved Currency", "Data Controller");
    }

    public static CurrencyToken LoadCurrency()
    {
        string currency_path = Application.persistentDataPath + "/currency" + m_PathEnding;
        if (File.Exists(currency_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _shard_stream = new FileStream(currency_path, FileMode.Open);

            CurrencyToken token = (CurrencyToken) _formatter.Deserialize(_shard_stream);

            _shard_stream.Close();

            Log("Loaded Currency", "Data Controller");


            return token;
        }
        else
        {
            Log("Save file not found in " + currency_path, "Data Controller");

            SaveCurrency(0);

            return LoadCurrency();
        }
    }

    public static void AddCurrency(int _shards, int[] fragments = null)
    {
        if (fragments == null)
        {
            fragments = new int[5];
        }

        CurrencyToken token = LoadCurrency();

        BinaryFormatter _formatter = new BinaryFormatter();
        string _shards_path = Application.persistentDataPath + "/currency" + m_PathEnding;
        FileStream _shards_stream = new FileStream(_shards_path, FileMode.Create);

        token.Shards = token.Shards + _shards;
        token.FragmentsTier1 += fragments[0];
        token.FragmentsTier2 += fragments[1];
        token.FragmentsTier3 += fragments[2];
        token.FragmentsTier4 += fragments[3];
        token.FragmentsTier5 += fragments[4];

        _formatter.Serialize(_shards_stream, token);
        _shards_stream.Close();
    }

    public static void SetCurrency(int _shards, int[] fragments = null) // Please you this for debuging purpose only.
    {
        if (fragments == null)
        {
            fragments = new int[5];
        }

        CurrencyToken token = LoadCurrency();

        BinaryFormatter _formatter = new BinaryFormatter();
        string _shards_path = Application.persistentDataPath + "/currency" + m_PathEnding;
        FileStream _shards_stream = new FileStream(_shards_path, FileMode.Create);

        token.Shards = _shards;
        token.FragmentsTier1 = fragments[0];
        token.FragmentsTier2 = fragments[1];
        token.FragmentsTier3 = fragments[2];
        token.FragmentsTier4 = fragments[3];
        token.FragmentsTier5 = fragments[4];

        _formatter.Serialize(_shards_stream, token);
        _shards_stream.Close();
    }


    #endregion

    #region Ship Data
    public static void SavePlayerShip(PlayerData _save)
    {
        // saves ship that is currently equiped
        if (_save == null)
        {
            LogWarning("You're trying to pass a null Player Stats Object", "Data Controller");
        }

        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + m_ShipEquiped  + m_PathEnding;
        FileStream _stream = new FileStream(_path, FileMode.Create);

        string ship_name = _save.name;

        _formatter.Serialize(_stream, ship_name);
        _stream.Close();

        Log("Saved Ship Data", "Data Controller");
    }

    public static PlayerData LoadPlayerShip()
    {
        string _path = Application.persistentDataPath + m_ShipEquiped + m_PathEnding;
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            string ship_name = _formatter.Deserialize(_stream) as string;
            _stream.Close();
            Log("Loaded Ship Data", "Data Controller");

            PlayerData player = Resources.Load<PlayerData>(m_ShipResourcePath + ship_name);

            return player;
        }
        else
        {
            LogWarning("Save file not found in " + _path, "Data Controller");
            PlayerData player = Resources.Load<PlayerData>(m_ShipResourcePath + "cova_ship");

            return player;
        }
    }
    #endregion

    #region Volume Data

    public static void SaveVolumeData(Hashtable _volumeTable) 
    {
        if (_volumeTable == null)
        {
            LogWarning("You're trying to pass a null Volume Table Object", "Data Controller");
        }
        string _path = Application.persistentDataPath + "/"+ m_VolumeFile + m_PathEnding;
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_path, FileMode.Create);

        VolumeData _data = new VolumeData(_volumeTable);

        _formatter.Serialize(_stream, _data);
        _stream.Close();

        Log("Saved Volume Data", "Data Controller");
    }

    public static Hashtable LoadVolumeData() 
    {
        string _path = Application.persistentDataPath + "/" + m_VolumeFile + m_PathEnding;
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            VolumeData data = _formatter.Deserialize(_stream) as VolumeData;
            _stream.Close();

            Log("Loaded Volume Data", "Data Controller");

            return data.volumeTable;
        }
        else
        {
            Hashtable volumeTable = new Hashtable();
            LogWarning("Save file not found in " + _path, "Data Controller");
            var values = Enum.GetValues(typeof(AudioTrackType));
            foreach (AudioTrackType _type in values)
            {
                volumeTable.Add(_type, 1f);
                
            }
            return volumeTable;
        }
    }

    public static void SaveMasterVolume(float _value) {
        string _path = Application.persistentDataPath + "/master_volume" + m_PathEnding;
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_path, FileMode.Create);

        _formatter.Serialize(_stream, _value);
        _stream.Close();

        Log($"Saved Master Volume Data [{_value}]", "Data Controller");
    }

    public static float LoadMasterVolume() {
        string _path = Application.persistentDataPath + "/master_volume" + m_PathEnding;
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            float data = (float) _formatter.Deserialize(_stream);
            _stream.Close();

            Log("Loaded Volume Data", "Data Controller");

            return data;
        }
        else
        {
            LogWarning("Save file not found in " + _path, "Data Controller");
            SaveMasterVolume(1f);
            return 1f;
        }
    }

    #endregion

    #region Player Attack

    public static void SavePlayerAttack(AttackData _attackData)
    {
        // Save a attack as the current equiped attack for the player
        if (_attackData == null)
        {
            LogWarning("You're trying to pass a null Attack Data Stats Object", "Data Controller");
        }

        string attackName = _attackData.name;

        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + m_AttackEquiped + m_PathEnding;
        FileStream _stream = new FileStream(_path, FileMode.Create);

        string _token = attackName;

        _formatter.Serialize(_stream, _token);
        _stream.Close();

        Log("Saved Player Attack Data", "Data Controller");

    }

    public static AttackData LoadPlayerAttack()
    {
        string _path = Application.persistentDataPath + m_AttackEquiped + m_PathEnding;
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            string attack_name = _formatter.Deserialize(_stream) as string;

            _stream.Close();
            Log("Loaded Player Attack Data", "Data Controller");

            AttackData attack = Resources.Load<AttackData>(m_AttacksResourcePath + attack_name);

            return attack;
        }
        else
        {
            LogWarning("Save file not found in " + _path, "Data Controller");
            AttackData attack = Resources.Load<AttackData>(m_AttacksResourcePath + m_DefaultAttack);

            return attack;
        }
    }

    #endregion

    #region Player Special

    public static void SavePlayerSpecial(SpecialData _specialData)
    {
        if (_specialData == null)
        {
            LogWarning("You're trying to pass a null Player Stats Object", "Data Controller");
        }

        string specialName = _specialData.name;

        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + m_SpecialEquiped + m_PathEnding;
        FileStream _stream = new FileStream(_path, FileMode.Create);

        string _token = specialName;

        _formatter.Serialize(_stream, _token);
        _stream.Close();

        Log("Saved Player Special Data", "Data Controller");
    }

    public static SpecialData LoadPlayerSpecial()
    {
        string _path = Application.persistentDataPath + m_SpecialEquiped + m_PathEnding;

        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            string special_name = _formatter.Deserialize(_stream) as string;

            _stream.Close();
            Log("Loaded Player Special Data", "Data Controller");

            SpecialData attack = Resources.Load<SpecialData>(m_SpecialsResourcePath + special_name);

            return attack;
        }
        else
        {
            LogWarning("Save file not found in " + _path + ". Loaded Default Data", "Data Controller");
            SpecialData attack = Resources.Load<SpecialData>(m_SpecialsResourcePath + m_DefaultSpecial);

            return attack;
        }
    }

    #endregion

    #region Player Shield

    public static void SavePlayerShield(ShieldData _shieldData)
    {
        if (_shieldData == null)
        {
            LogWarning("You're trying to pass a null Player Stats Object", "Data Controller");
        }

        string shieldName = _shieldData.name;

        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + m_ShieldEquiped + m_PathEnding;
        FileStream _stream = new FileStream(_path, FileMode.Create);

        string _token = shieldName;

        _formatter.Serialize(_stream, _token);
        _stream.Close();

        Log("Saved Player Shield Data", "Data Controller");
    }

    public static ShieldData LoadPlayerShield()
    {
        string _path = Application.persistentDataPath + m_ShieldEquiped + m_PathEnding;

        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            string shield_name = _formatter.Deserialize(_stream) as string;

            _stream.Close();
            Log("Loaded Player Shield Data", "Data Controller");

            ShieldData shield = Resources.Load<ShieldData>(m_ShieldsResourcePath + shield_name);

            return shield;
        }
        else
        {
            LogWarning("Save file not found in " + _path + ". Loaded Default Data", "Data Controller");
            ShieldData shield = Resources.Load<ShieldData>(m_ShieldsResourcePath + m_DefaultShield);

            return shield;
        }
    }

    #endregion

    #region Logger
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(string _msg, string origin) 
    {
        Debug.Log(string.Format("[{0}] {1}", origin, _msg));
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogWarning(string _msg, string origin)
    {
        Debug.LogWarning(string.Format("[{0}] {1}", origin, _msg));
    }
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogError(string _msg, string origin)
    {
        Debug.LogError(string.Format("[{0}] {1}", origin, _msg));
    }
    #endregion
    #endregion
}
}