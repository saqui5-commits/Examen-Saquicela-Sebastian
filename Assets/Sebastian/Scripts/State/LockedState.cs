using UnityEngine;
namespace Sebastian {

    public class LockedState : IChestState
    {
        private Chest chest;

        public LockedState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            Debug.Log("Cofre: BLOQUEADO");
            chest.SetLid(false);
            chest.SetLockedVisual(true);
            chest.SetColor(new Color(1f, 0.45f, 0.45f));
        }

        public void Update() { }

        public void OnExit()
        {
            Debug.Log("Saliendo de BLOQUEADO");
            chest.SetLockedVisual(false);
        }

        public void Interact()
        {
            // Impide la apertura: el cofre se sacude
            chest.PlayDenied();
            Debug.Log("El cofre esta bloqueado. Presiona U para desbloquear.");
        }
    }
}
