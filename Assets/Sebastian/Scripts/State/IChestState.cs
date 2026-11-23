using UnityEngine;
namespace Sebastian
{
    public interface IChestState
   {
       void OnEnter();     
       void Update();      
       void OnExit();      
       void Interact();    
   }
}