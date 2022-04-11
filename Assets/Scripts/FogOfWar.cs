using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// This class builds the fog of war based on tilemaps
/// </summary>
public class FogOfWar : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;
    public Transform startPos;
    private float exposurRad = 0.5f; //how much a unit can expose of the tile on top of it 
    private float cellSize = 10;
    private float timer;
    // Start is called before the first frame update
    void Start() {
        createTiles(80,80, tilemap, tile);

        //expose some tiles at the start of the level
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level == 1 || level == 3 ) {
            exposteTilesAround(startPos.position / cellSize);
        }
        
    }


    /// <summary>
    /// this method creates tiles on a tilemap
    /// </summary>
    /// <param name="width"></param> how many tiles in width
    /// <param name="height"></param> how many tiles in height
    /// <param name="tilemap"></param> the tilemap to set the tiles on
    /// <param name="tile"></param> the tile base
    public static void createTiles(int width, int height, Tilemap tilemap, TileBase tile) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void Update() {

        //check if any unit is under a tile and only update tiles
        //once ever 0.5 second
        if (timer <= 0) {
            timer = 0.5f;

            //get all player units
            PlayerUnit[] units = LevelManager.manager.getUnits().ToArray();
            foreach (PlayerUnit unit in units) {
                Vector3 center = unit.transform.position;
                center = center / cellSize;
                exposteTilesAround(center); //check if the unit is under a set of tiles
            }

            //get all player buildings
            Transform[] tr = LevelManager.manager.getbuildings().ToArray();
            foreach (Transform unit in tr) {
                Vector3 center = unit.transform.position;
                center = center / cellSize;
                exposteTilesAround(center); //check if the unit is under a set of tiles
            }

        }

        timer -= Time.deltaTime;

    }

    /// <summary>
    /// this method exposes tiles ontop of and around a given position
    /// </summary>
    /// <param name="center"></param> the center of tiles exposure
    private void exposteTilesAround(Vector3 center) {
        tilemap.SetTile(new Vector3Int((int)(center.x + exposurRad), (int)(center.z + exposurRad), 0), null);
        tilemap.SetTile(new Vector3Int((int)(center.x - exposurRad), (int)(center.z + exposurRad), 0), null);
        tilemap.SetTile(new Vector3Int((int)(center.x + exposurRad), (int)(center.z - exposurRad), 0), null);
        tilemap.SetTile(new Vector3Int((int)(center.x - exposurRad), (int)(center.z - exposurRad), 0), null);
        tilemap.SetTile(new Vector3Int((int)center.x, (int)center.z, 0), null);
    }
}
