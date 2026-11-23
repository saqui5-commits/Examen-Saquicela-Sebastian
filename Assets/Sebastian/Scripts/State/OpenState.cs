using UnityEngine;
namespace Sebastian {

    // Estado ABIERTO: muestra el contenido y permite cerrarse
    public class OpenState : IChestState
    {
        private Chest chest;

        public OpenState(Chest chest)
        {
            this.chest = chest;
        }

        public void OnEnter()
        {
            Debug.Log("Cofre: ABIERTO - muestra el contenido");
            chest.SetLid(true);
            chest.SetColor(Color.white);
        }

        public void Update() { }

        public void OnExit()
        {
            Debug.Log("Saliendo de ABIERTO");
        }

        public void Interact()
        {
            // Cerrar el cofre
            chest.ChangeState(chest.ClosedState);
        }
    }
}