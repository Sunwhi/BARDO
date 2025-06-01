using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerBase : MonoBehaviour
{
    [Tooltip("�� ���� �ߵ��Ǵ� Ʈ�������� ����")]
    [SerializeField] private bool oneTimeTrigger = true;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (oneTimeTrigger && hasTriggered) return;

        hasTriggered = true;
        OnTriggered();
    }

    protected abstract void OnTriggered();
}