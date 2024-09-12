
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System.Collections;

public class GridSelection : MonoBehaviour
{
    [SerializeField] GameObject Next, Prev, GridScroll;
    RectTransform GridScrollRect;
    Button NextButton, PrevButton;

    public GameObject SpawnGrid = null;
    // Start is called before the first frame update


    void OnEnable()
    {
        StartCoroutine(RegiSterEventWithDelay());
    }

    IEnumerator RegiSterEventWithDelay()
    {
        yield return GameUIControl.Instance != null;
        GameUIControl.Instance.DestroyorRefreshGrid += DestroyorRefreshGrid;
    }

    void OnDisable()
    {
        GameUIControl.Instance.DestroyorRefreshGrid -= DestroyorRefreshGrid;
    }
    void Start()
    {
        GridScrollRect = GridScroll.GetComponent<RectTransform>();
        NextButton = Next.GetComponent<Button>();
        PrevButton = Prev.GetComponent<Button>();
    }

    bool Ismoving = false;
    public void ButtonClick(float DistTomovbe)
    {
        Debug.Log("==>on button Click|" + DistTomovbe + "::" + (GridScrollRect == null));
        float Dist = GridScrollRect.anchoredPosition.x + DistTomovbe;
        SetButtonVisibility(Dist);
        if (!Ismoving)
        {
            Ismoving = true;
            GridScrollRect.DOAnchorPosX(Dist, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Ismoving = false;
            });
        }
    }

    public void LoadLevel(string Name)
    {
        Debug.Log("==>Load Level|" + Name + "::" + Ismoving);
        if (!Ismoving)
        {
            SpawnGrid = Instantiate(Resources.Load<GameObject>(Name));
            SpawnGrid.name = Name;
            SpawnGrid.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            Invoke("SetActivePlayerInput", 1f);
        }
    }

    void SetActivePlayerInput()
    {
        PlayerInfo.Instance.PlayerInput = true;
    }

    void SetButtonVisibility(float Position)
    {
        if (Position >= 2625F)
        {
            PrevButton.enabled = false;
            NextButton.enabled = true;
            Prev.GetComponent<CanvasGroup>().alpha = 0.5f;
            return;
        }
        else if (Position <= -2625F)
        {
            PrevButton.enabled = true;
            NextButton.enabled = false;
            Next.GetComponent<CanvasGroup>().alpha = 0.5f;

        }
        else
        {
            Prev.GetComponent<Button>().enabled = true;
            Next.GetComponent<Button>().enabled = true;
            Prev.GetComponent<CanvasGroup>().alpha = 1f;
            Next.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    void DestroyorRefreshGrid(bool IsRefresh)
    {
        if (SpawnGrid != null)
        {
            string GridName = SpawnGrid.name;
            Debug.Log("DestRoy");
            Destroy(SpawnGrid);
            if (IsRefresh)
            {
                GameUIControl.Instance.Loading.SetActive(true);
                StartCoroutine(LoadLevelLate(GridName));
            }
        }
    }

    IEnumerator LoadLevelLate(string GridName)
    {
        yield return new WaitForSeconds(0.5f);
        GameUIControl.Instance.Loading.SetActive(false);
        LoadLevel(GridName);

    }
}
