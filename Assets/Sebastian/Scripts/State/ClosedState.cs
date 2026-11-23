using UnityEngine;

namespace Sebastian {
    public class ClosedState : IChestState
    {
        private Chest chest;

        public ClosedState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            Debug.Log("Cofre: CERRADO");
            chest.SetLid(false);
            chest.SetColor(Color.white);
        }

        public void Update() { }

        public void OnExit()
        {
            Debug.Log("Saliendo de CERRADO");
        }

        public void Interact()
        {
            // Solo se abre si no esta bloqueado
            if (!chest.IsLocked)
                chest.ChangeState(chest.OpenState);
        }
    }
}