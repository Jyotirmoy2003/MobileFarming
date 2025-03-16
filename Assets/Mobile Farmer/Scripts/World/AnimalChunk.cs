using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalChunk : Chunk
{
    [SerializeField] AnimalBase animalBase;

    private FeedBackManager feedBackManager;

    void Start()
    {
        feedBackManager.CompletePlayingFeedback += UnlockAnimal;
    }

    void UnlockAnimal()
    {
        feedBackManager.CompletePlayingFeedback -= UnlockAnimal;
        
    }
}
