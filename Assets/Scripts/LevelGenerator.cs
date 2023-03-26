using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator sharedInstance;

    //Esta lista almacena todos los disponibles, osea todos los creados como prefabs
    [SerializeField] private List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    [SerializeField] private Transform levelStartPoint;

    //Esta lista se rellena dinamicamente
    [SerializeField] private List<LevelBlock> currentBlocks = new List<LevelBlock>();
    [SerializeField] private LevelBlock firstBlock;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        GenerateInitialBlocks();
    }

    public void AddLevelBlock(){
        //Elige un bloque random
        int randomIndex = Random.Range(0, allTheLevelBlocks.Count);

        //Instancia ese bloque elegido y lo hace hijo de este levelgenerator
        LevelBlock currentBlock; 

        //Si recien arranca el juego uso vector 0, sino el start position del bloque siguiente
        Vector3 spawnPosition = Vector3.zero;
        if(currentBlocks.Count == 0){
            //Si es el primero que genero, quiero que sea el bloque que yo elijo
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = levelStartPoint.position;
        }else{
            currentBlock = (LevelBlock)Instantiate(allTheLevelBlocks[randomIndex]);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;
        }

        //Este es el vector que me corrige los bloques de escena para que queden alineados correctamente
        Vector3 correction = new Vector3(spawnPosition.x - currentBlock.startPoint.position.x, spawnPosition.y - currentBlock.startPoint.position.y , 0f);

        //Lo pone en el mundo y lo agrega a la lista dinamica
        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);
    }

    public void RemoveOldestLevelBlock(){
        LevelBlock oldestBlock = currentBlocks[0];
        currentBlocks.Remove(oldestBlock);
        Destroy(oldestBlock.gameObject);
    }

    public void RemoveAllTheBlocks(){
        while(currentBlocks.Count > 0){
            RemoveOldestLevelBlock();
        }
    }

    public void GenerateInitialBlocks(){
        for (var i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}