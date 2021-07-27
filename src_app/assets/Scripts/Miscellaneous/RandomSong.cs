using UnityEngine;
using System.Collections;

public class RandomSong : MonoBehaviour {

    public AudioClip[] songs;
    public float[] probabilities;
    public IntroAnimation script;

    public SpriteRenderer uni, sun;

	void Awake ()
    {
        GetComponent<AudioSource>().clip = songs[ChooseRandomWithProbabilities()];
        GetComponent<AudioSource>().Play();
    }

    void LateUpdate ()
    {
        uni.color = new Vector4 (uni.color.r, uni.color.g, uni.color.b,script.alphaBackground);
        sun.color = new Vector4(sun.color.r, sun.color.g, sun.color.b, script.alphaBackground);
    }

    int ChooseRandomWithProbabilities()
    {
        float total = 0;

        foreach (float i in probabilities)
            total += i;

        float r = Random.value * total;

        for (int i = 0; i < probabilities.Length; i++)
        {
            if (r < probabilities[i])
                return i;
            else
                r -= probabilities[i];
        }
        return probabilities.Length - 1;
    }
}
