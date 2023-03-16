using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TileScript : MonoBehaviour
{
    #region VARIABLES

    private float FallDelay = 0.2f;
    private float ReuseDelay = 2f;

    #endregion
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TileManager.InstanceTM.spawnTile();
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown()
    {
        yield return new WaitForSeconds(FallDelay);
        GetComponent<Rigidbody>().isKinematic = false;
        
        // to reuse the tiles
        yield return new WaitForSeconds(ReuseDelay);
        
        switch (gameObject.name)
        {
            case "LeftTile":
                TileManager.InstanceTM.leftTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            
            case "TopTile":
                TileManager.InstanceTM.topTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
        }
    }

    
}
