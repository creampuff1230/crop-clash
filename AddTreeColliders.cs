using UnityEngine;

public class AddTreeColliders : MonoBehaviour
{
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] trees = terrainData.treeInstances;
        TreePrototype[] prototypes = terrainData.treePrototypes;


        foreach (TreeInstance tree in trees)
        {
            // Get the world position of the tree
            Vector3 worldPosition = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;

            // Get the tree prefab and instantiate it
            GameObject treePrefab = prototypes[tree.prototypeIndex].prefab;

            GameObject treeInstance = Instantiate(treePrefab, worldPosition, Quaternion.identity);

            // try to fix rendering error in console



            treeInstance.transform.parent = this.transform; // Parent it for organization


            if (treePrefab.name == "mushroom_tanTall") continue;

            // Add a collider if it doesn't exist
            if (!treeInstance.GetComponent<Collider>())
            {
                CapsuleCollider collider = treeInstance.AddComponent<CapsuleCollider>();
                collider.center = Vector3.zero;
                collider.height = 5.0f; // Adjust this based on your tree size
                collider.radius = 1.0f; // Adjust this based on your tree trunk width
            }

        }

        // Disable the original terrain trees to prevent duplicates
        terrain.drawTreesAndFoliage = false;
    }

   
}
