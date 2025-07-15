using System.Collections;

namespace CF.Data {
[System.Serializable]
public class VolumeData
{
    public Hashtable volumeTable;

    public VolumeData(Hashtable _volumeTable) 
    {
        volumeTable = new Hashtable();
        volumeTable = _volumeTable;
    }
}
}