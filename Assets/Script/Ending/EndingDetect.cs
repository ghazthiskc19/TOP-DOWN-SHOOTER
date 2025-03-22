using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndingDetect : MonoBehaviour
{
    string[] DocsTittle = new string[] {
        "Military Tribunal Order No. 1987 – Charges of Desertion & Treason",
        "Medical Commendation No. 211 – Acts of Valor Under Fire",
        "Psychiatric Report No. 44 – Case Study: Battlefield Trauma",
        "Service Record No. 317 – Duty Acknowledged"
    };
    string[] NarrativeTexts = new string[] {
        "During the brutal trench battles of the Siege of Zimograd, Aleksi Ivanov was found guilty of desertion. As artillery fire pounded the medical stations, he fled his post, leaving wounded comrades to their fate in the mud-choked trenches. For three days, he evaded capture before being discovered in an abandoned dugout. At Night, he was executed by firing squad as a warning to others. His family was stripped of all civil rights, his name stricken from military records. His remains were left unclaimed beyond the wirefields of Zimograd.",
        "Under relentless artillery fire during the Siege of Zimograd, Aleksi Ivanov defied orders to retreat, instead turning a collapsing bunker into an improvised field hospital. With supplies dwindling, he performed surgeries under candlelight, stabilizing wounded soldiers who would have otherwise perished in the freezing trenches. When the outer defenses fell, he carried the last of the injured through collapsing tunnels. In recognition of his bravery, he was awarded the Iron Heart medal and reassigned to a secure medical station. Years later, he was honored in the capital, where his name became synonymous with duty and compassion. He never returned to the front, but survivors of Zimograd would always remember ‘The Healer of the Trenches.’",
        "After 18 months in the frozen hell of the Siege of Zimograd, Aleksi Ivanov’s mind unraveled. Witnesses reported that he spoke to soldiers who had already died, apologizing for his ‘failures.’ He was found in a collapsed dugout, muttering over the bodies of fallen comrades, refusing to leave. Diagnosed with acute battlefield trauma, he was sent to the Hinterland Military Asylum, where he spent his days drawing the faces of those he could not save. His final words, spoken in a whisper, were ‘I should have done more.’ He passed away in the winter of 1917. No grave was marked, no honors bestowed.",
        "Throughout the relentless trench battles of the Siege of Zimograd, Aleksi Ivanov maintained his commitment as a medic with unwavering resolve. His consistent, measured actions—tending to the wounded amid chaos and despair—proved vital in keeping his unit moving, though they never shifted the tide of battle. While no single moment defined his service as heroic, his duty was performed with honor and integrity. His efforts were recorded in the service logs of the 11th Battalion, recognized not with a medal but with a quiet acknowledgment that every soldier’s duty sustains the larger war effort."
    };
    public Image[] endingVariants;
    [Header("Tampilkan Ending")]
    public TMP_Text _docsText;
    public Image _variantImage;
    public TMP_Text _narrativeText;
    public Image LogoFlagImage;
    public TMP_Text LogoDescriptionText;
    public Button TryAgain;
    [Header("Semua Canvas Group Obj")]
    public CanvasGroup[] allCanvasGroup;
    public CanvasGroup canvasWrapper;
    public GameObject EndingVariant;
    [Space(10)]
    public Canvas EndingWrapper;
    public float tDuration;
    public float tDelay;
    public float typingDuration;
    public UnityEvent DisableAllFunction;
    private PlayerMovement pm;
    private string LogoDescription = "Long Live Vostriha";
    private void Awake()
    {
        TryAgain.gameObject.SetActive(false);
        canvasWrapper.alpha = 0f;
        EndingWrapper.gameObject.SetActive(false);
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        foreach(Image obj in endingVariants){
            obj.gameObject.SetActive(false);
        }
        ResetCanvasGroup();
    }

    private void ResetCanvasGroup(){
        foreach(CanvasGroup obj in allCanvasGroup){
            obj.alpha = 0f;
            obj.interactable = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            EndingWrapper.gameObject.SetActive(true);
            TransitionOverlay();
        }
    }

    private void TransitionOverlay()
    {
        LeanTween.alphaCanvas(canvasWrapper, 1, 1f)
        .setOnComplete(
            () => {
                StartCoroutine(PlayEndingSequence());
            }
        );
    }
    private IEnumerator PlayEndingSequence()
    {
        DisableAllFunction?.Invoke();
        LeanTween.alphaCanvas(canvasWrapper, 1, 1f);
        yield return new WaitForSeconds(1f);

        int index = GettingIndex();
        Debug.Log("indeks : " + index);
        _docsText.text = DocsTittle[index];
        _variantImage.sprite = endingVariants[index].sprite;
        CanvasGroup varImage = _variantImage.gameObject.GetComponent<CanvasGroup>();
        CanvasGroup narrativeImage  = _narrativeText.gameObject.GetComponent<CanvasGroup>();

        // layer docs muncul
        LeanTween.alphaCanvas(allCanvasGroup[0], 1, 0);

        // animation tittle docs muncul
        StartCoroutine(TypeWriterEffect(_docsText, _docsText.text, typingDuration));
        yield return new WaitForSeconds(typingDuration * 2);

        // animation tittle docs hilang
        LeanTween.alphaCanvas(_docsText.GetComponent<CanvasGroup>(), 0, 1f);
        yield return new WaitForSeconds(2f);

        // layer docs hilang, layer narrative and image muncul
        LeanTween.alphaCanvas(allCanvasGroup[0], 0, 0);
        LeanTween.alphaCanvas(allCanvasGroup[1], 1, 1f);
        yield return new WaitForSeconds(1f);

        // layer image and narrative muncul
        LeanTween.alphaCanvas(varImage, 1, 1f);
        LeanTween.alphaCanvas(narrativeImage, 1, 1f).setDelay(.5f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(TypeWriterEffect(_narrativeText, NarrativeTexts[index], 6f));
        yield return new WaitForSeconds(12f);

        // layer image and narrative hilang
        LeanTween.alphaCanvas(varImage, 0, 1f);
        LeanTween.alphaCanvas(narrativeImage, 0, 1f).setDelay(.25f);
        yield return new WaitForSeconds(2f);

        TryAgain.gameObject.SetActive(true);
        LeanTween.alphaCanvas(TryAgain.GetComponent<CanvasGroup>(), 1f, 1f);
        LeanTween.alphaCanvas(allCanvasGroup[1], 0, 0);
        LeanTween.alphaCanvas(allCanvasGroup[2], 1, 0);

        LeanTween.alphaCanvas(LogoFlagImage.GetComponent<CanvasGroup>(), 1, 1f);
        StartCoroutine(TypeWriterEffect(LogoDescriptionText, LogoDescription, 2f));
        yield return new WaitForSeconds(4f);

        // Reset all canvas group to default
        LeanTween.alphaCanvas(varImage, 0, 0);
        LeanTween.alphaCanvas(narrativeImage, 0, 0);
    }

    private int GettingIndex()
    {
        if(PlayerInformation.instance.currentCure == 0){
            return 0;
        }
        else if (PlayerInformation.instance.currentCure >= 3){
            return 1;
        }else if(SanityController.instance.CurrentSanity >= 200){
            return 2;
        }else{
            return 3;
        }
    }
    public void RemoveEndingScene()
    {
        LeanTween.alphaCanvas(canvasWrapper, 0, 0).setOnComplete(
            () => {
                EndingWrapper.gameObject.SetActive(false);
            }
        );  
        TryAgain.gameObject.SetActive(false);
        // Bagian slide terakhir nih, harus semuanya di reset di tempat yang beda
        // kalau gak mau keganggu scenenya
        LeanTween.alphaCanvas(TryAgain.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.alphaCanvas(allCanvasGroup[2], 0, 1f);
        LeanTween.alphaCanvas(LogoFlagImage.GetComponent<CanvasGroup>(), 0, 0);
        LogoDescriptionText.text = "";
        ResetCanvasGroup();
        LeanTween.alphaCanvas(_docsText.GetComponent<CanvasGroup>(), 1, 0);
    }

    private IEnumerator TypeWriterEffect(TMP_Text textComponent, string fullText, float typingDuration )
    {

        textComponent.text = "";
        SoundManager.instance.PlayLoopingSFX(SoundManager.instance.TypingSound);
        foreach(char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingDuration / fullText.Length);
        }
        SoundManager.instance.StopLoopingSFX();
    }
}
