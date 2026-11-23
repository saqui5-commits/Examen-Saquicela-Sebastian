using UnityEngine;
namespace Sebastian {

    // Contexto del patron State: administra el estado actual del cofre
    public class Chest : MonoBehaviour
    {
        [Header("Teclas")]
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        [SerializeField] private KeyCode lockKey = KeyCode.L;
        [SerializeField] private KeyCode unlockKey = KeyCode.U;

        [Header("Visual")]
        [SerializeField] private Transform lid;            // Opcional: tapa animada por transformacion (si no hay Animator)
        [SerializeField] private float openAngle = -100f;
        [SerializeField] private float lidSpeed = 5f;
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Animator animator;

        [Header("Debug")]
        [SerializeField] private string currentStateName;
        [SerializeField] private bool isLocked;

        // Parametros del Animator
        private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
        private static readonly int IsLockedHash = Animator.StringToHash("IsLocked");
        private static readonly int DeniedHash = Animator.StringToHash("Denied");

        // Estado actual y los tres estados
        private IChestState currentState;
        private ClosedState closedState;
        private OpenState openState;
        private LockedState lockedState;

        private Quaternion lidClosedRot;
        private Quaternion lidTargetRot;

        // Acceso para los estados
        public ClosedState ClosedState => closedState;
        public OpenState OpenState => openState;
        public LockedState LockedState => lockedState;
        public bool IsLocked => isLocked;

        private void Start()
        {
            if (lid != null)
            {
                lidClosedRot = lid.localRotation;
                lidTargetRot = lidClosedRot;
            }

            // Inicializar los tres estados
            closedState = new ClosedState(this);
            openState = new OpenState(this);
            lockedState = new LockedState(this);

            // Estado inicial
            ChangeState(closedState);
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactionKey)) Interact();
            if (Input.GetKeyDown(lockKey)) Lock();
            if (Input.GetKeyDown(unlockKey)) Unlock();

            currentState?.Update();

            // Animacion de la tapa por transformacion (solo si se asigno una tapa)
            if (lid != null)
                lid.localRotation = Quaternion.Slerp(lid.localRotation, lidTargetRot, Time.deltaTime * lidSpeed);
        }

        // Cambia de estado ejecutando OnExit del anterior y OnEnter del nuevo
        public void ChangeState(IChestState newState)
        {
            if (newState == null || newState == currentState) return;

            currentState?.OnExit();
            currentState = newState;
            currentStateName = newState.GetType().Name;
            currentState.OnEnter();
        }

        // El estado actual decide que hacer
        public void Interact()
        {
            currentState?.Interact();
        }

        public void Lock()
        {
            // Solo se puede bloquear si esta cerrado
            if (currentState != closedState)
            {
                Debug.Log("Cierra el cofre antes de bloquearlo.");
                return;
            }
            isLocked = true;
            ChangeState(lockedState);
        }

        public void Unlock()
        {
            if (currentState != lockedState) return;
            isLocked = false;
            ChangeState(closedState);
        }

        // ---- Ayudas visuales que usan los estados ----
        public void SetLid(bool open)
        {
            if (lid != null)
                lidTargetRot = open ? lidClosedRot * Quaternion.Euler(openAngle, 0f, 0f) : lidClosedRot;

            if (animator != null)
                animator.SetBool(IsOpenHash, open);
        }

        public void SetLockedVisual(bool locked)
        {
            if (animator != null)
                animator.SetBool(IsLockedHash, locked);
        }

        // Animacion de "no se puede abrir" (sacudida) cuando esta bloqueado
        public void PlayDenied()
        {
            if (animator != null)
                animator.SetTrigger(DeniedHash);
        }

        public void SetColor(Color c)
        {
            if (bodyRenderer != null)
                bodyRenderer.material.color = c;
        }
    }
}
