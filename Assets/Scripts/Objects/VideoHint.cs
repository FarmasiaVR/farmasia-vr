using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoHint : MonoBehaviour {

    #region fields
    public static VideoHint CurrentVideo;
    public static GameObject videoHintPrefab;

    private VideoPlayer player;
    private AudioSource audioSource;
    private Renderer targetRenderer;
    private Material targetMaterial;
    private Color matColor;

    private VideoClip clip;

    private TextMeshPro text;

    private float spawnTime = 0.5f;
    private Vector3 targetSize;
    private Transform closeBtn;
    private Transform playBtn;
    private DragAcceptable closeButton;
    private DragAcceptable playButton;
    private string videoTitle;
    #endregion

    #region Initialization
    private void Awake() {
        Transform v = transform.Find("Video");
        player = v.GetComponent<VideoPlayer>();
        audioSource = v.GetComponent<AudioSource>();
        targetRenderer = v.GetComponent<Renderer>();
        targetMaterial = targetRenderer.material;
        matColor = targetMaterial.color;
        closeBtn = transform.Find("CloseButton");
        playBtn = transform.Find("PlayButton");
        text = transform.Find("Text").GetComponent<TextMeshPro>();

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    private void Start() {
        text.text = videoTitle;
        StartCoroutine(InitSpawn());
    }

    private void PlayVideo() {

        Logger.Print("Playing video: " + clip);

        text.gameObject.SetActive(false);

        playButton.Hide(true);

        player.clip = clip;
        player.frame = 0;
        player.renderMode = VideoRenderMode.MaterialOverride;
        player.targetMaterialRenderer = targetRenderer;
        player.targetMaterialProperty = "_MainTex";
        player.audioOutputMode = VideoAudioOutputMode.AudioSource;
        player.SetTargetAudioSource(0, audioSource);
        player.isLooping = false;

        player.loopPointReached -= VideoEnded;
        player.loopPointReached += VideoEnded;

        targetMaterial.color = Color.white;

        player.Play();
    }

    private void VideoEnded(VideoPlayer vp) {
        text.gameObject.SetActive(true);
        playButton.Hide(false);
        targetMaterial.color = matColor;
    }

    private IEnumerator InitSpawn() {

        float time = spawnTime;

        Vector3 closeButtonPos = closeBtn.localPosition;
        Vector3 playButtonPos = playBtn.localPosition;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = 1 - time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }

        transform.localScale = targetSize;
        closeBtn.localPosition = closeButtonPos;
        closeBtn.transform.parent = null;

        playBtn.localPosition = playButtonPos;
        playBtn.transform.parent = null;

        closeBtn.gameObject.AddComponent<Rigidbody>().useGravity = false;
        closeButton = closeBtn.gameObject.AddComponent<DragAcceptable>();
        closeButton.LookAtPlayer = true;
        closeButton.OnAccept = DestroyHint;
        closeButton.Disabled = false;

        playBtn.gameObject.AddComponent<Rigidbody>().useGravity = false;
        playButton = playBtn.gameObject.AddComponent<DragAcceptable>();
        playButton.LookAtPlayer = true;
        playButton.OnAccept = PlayVideo;
        playButton.Disabled = false;
        playButton.ReleaseAfterActivate = true;
    }

    public void DestroyHint() {

        StopAllCoroutines();

        Destroy(player);
        Destroy(audioSource);

        if (closeButton != null) {
            closeButton.SafeDestroy();
        }
        if (playButton != null) {
            playButton.SafeDestroy();
        }

        GetComponent<Interactable>().DestroyInteractable();
    }

    private static void Init() {
        if (videoHintPrefab == null) {
            videoHintPrefab = Resources.Load<GameObject>("Prefabs/VideoHint");
        }
    }
    #endregion



    public static void CreateVideoHint(VideoClip clip, string videoTitle, Vector3 position) {

        if (clip == null) {
            return;
        }

        if (CurrentVideo != null) {
            CurrentVideo.DestroyHint();
        }

        Init();

        GameObject newHint = Instantiate(videoHintPrefab);
        newHint.transform.position = position;

        CurrentVideo = newHint.GetComponent<VideoHint>();

        CurrentVideo.clip = clip;
        CurrentVideo.videoTitle = videoTitle;
    }
}