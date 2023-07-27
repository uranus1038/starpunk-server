using System.Numerics;
namespace UMI.Network.Server
{
    // Setting Player
    public class UMIPlayer
    {
        public static UMIPlayer star;
        public int UID;
        public string userName;
        public Vector3 position;
        public Quaternion rotation;
        public string gender;
        public UMIPlayer(int UID, string userName, Vector3 spawnPosition, string gender)
        {
            this.UID = UID;
            this.userName = userName;
            this.position = spawnPosition;
            this.rotation = Quaternion.Identity;
            this.gender = gender;
        }
        public void UMIUpdate()
        {

        }
        public void resPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}