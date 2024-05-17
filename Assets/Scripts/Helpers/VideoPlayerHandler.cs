using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerHandler : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string videoFileName;


    private void OnEnable()
    {
        PlayVideo();
    }
    private void PlayVideo()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = path;
        videoPlayer.Play();
    }
}
