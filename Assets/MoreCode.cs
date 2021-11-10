using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class MoreCode : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;
    public AudioSource WhaleSounds;
    public AudioClip[] Wales;
    public KMSelectable[] whales;
    public Material[] BlueWhales;
    public TextMesh[] TextWhales;
    public GameObject TheBlueWhale;
    public GameObject SKIP;
    int MyWhale;
    int WhaleWhaleWhale;
    bool WhaliureCheck = false;
    string[] True = { "Largest animal on earth", "Can't eat anything bigger than a grapefruit", "Can hear up to 1600km away", "Listed as endangered", "Can weigh up to 200 tons", "Tongue is as heavy as an elephant", "Largest heart in the world", "Sometimes attacked by orcas" };
    string[] False = { "2nd largest animal on earth", "Can't eat anything bigger than a cantaloupe", "Can hear up to 1600m away", "Listed as vulnerable", "Longest animal on earth", "Largest living thing on earth", "Fastest thing in the natural world", "Biggest dead body in the world", "Loudest marine animal", "Biggest mouth" };
    string[] GoodWhales = { "Largest animal\non earth", "Can't eat anything\nbigger than a\ngrapefruit", "Can hear up to\n1600km away", "Listed as\nendangered", "Can weigh up\nto 200 tons", "Tongue is as\nheavy as an\nelephant", "Largest heart\nin the world", "Sometimes\nattacked by\norcas" };
    string[] BadWhales = { "2nd largest\nanimal on earth", "Can't eat anything\nbigger than a\ncantaloupe", "Can hear up to\n1600m away", "Listed as\nvulnerable", "Longest animal\non earth", "Largest living\nthing on earth", "Fastest thing in\nthe natural world", "Biggest dead\nbody in the\nworld", "Loudest marine animal", "Biggest mouth" };
    int[] WhalePositions = new int[4];

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
        SKIP.SetActive(false);
        for (byte i = 0; i < whales.Length; i++)
        {
            KMSelectable WhaleNumber = whales[i];
            WhaleNumber.OnInteract += delegate
            {
                WhalePress(WhaleNumber);
                return false;
            };
        }
        TheBlueWhale.GetComponent<MeshRenderer>().material = BlueWhales[UnityEngine.Random.Range(0, 10)];
        MyWhale = UnityEngine.Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            WhalePositions[i] = 100;
        }
        for (int i = 0; i < 4; i++)
        {
                if (i == MyWhale)
                    WhalePositions[i] = UnityEngine.Random.Range(0, GoodWhales.Length);
                else
                    WhalePositions[i] = UnityEngine.Random.Range(0, BadWhales.Length);
        }
        for (int i = 0; i < 4; i++)
        {
            if (i == MyWhale)
            {
                TextWhales[i].text = GoodWhales[WhalePositions[i]];
                Debug.LogFormat("[Blue Whale #{0}] Whale's button number {1} currently says '{2}'.", moduleId, i + 1, True[WhalePositions[i]]);
            }
            else
            {
                TextWhales[i].text = BadWhales[WhalePositions[i]];
                Debug.LogFormat("[Blue Whale #{0}] Whale's button number {1} currently says '{2}'.", moduleId, i + 1, False[WhalePositions[i]]);
            }
        }
        Debug.LogFormat("[Blue Whale #{0}] Knowing this, position {1} is the correct button to select.", moduleId, MyWhale + 1);
    }

    void WhalePress (KMSelectable WhaleNumber) {
        int whaley = Array.IndexOf(whales, WhaleNumber);
        whales[whaley].AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, WhaleNumber.transform);
        if (whaley == 4)
        {
            WhaleSounds.Stop();
        }
        else if (!moduleSolved)
        {
            if (whaley == MyWhale)
                Debug.LogFormat("[Blue Whale #{0}] Whale is happy you know about him. Solved.", moduleId);
            else
                Debug.LogFormat("[Blue Whale #{0}] Your choice of position {1} ({2}) was incorrect, and Whale is sad. Why would you do that? Strike.", moduleId, whaley + 1, False[WhalePositions[whaley]]);
            if (whaley == MyWhale)
            {
                StartCoroutine(WhaleSolve());
                WhaleSounds.PlayOneShot(Wales[11]);
                moduleSolved = true;
            }
            else
            {
                Module.HandleStrike();
                WhaleSounds.PlayOneShot(Wales[UnityEngine.Random.Range(0,11)]);
            }
        }
    }

    IEnumerator WhaleSolve()
    {
        yield return new WaitForSeconds(5.5f);
        TextWhales[0].text = "NICE";
        TextWhales[1].text = "YOU'VE";
        TextWhales[2].text = "DONE";
        TextWhales[3].text = "IT";
        Module.HandlePass();
        TheBlueWhale.GetComponent<MeshRenderer>().material = BlueWhales[10];
        SKIP.SetActive(true);
        while (WhaleSounds.isPlaying)
            yield return new WaitForSeconds(0.01f);
        TheBlueWhale.GetComponent<MeshRenderer>().material = BlueWhales[UnityEngine.Random.Range(0, 10)];
        SKIP.SetActive(false);
    }

    /*/ i am bad at tp support apparently but whatevs I guess
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} answer [1-4] (Selects the specified answer in reading order)";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand (string command) {
        string[] WhalePlays = command.Split(' ');
        if (Regex.IsMatch(WhalePlays[0], @"^\s*answer\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            try
            {
                int TemporaryWhale = Int32.Parse(WhalePlays[1]);
                WhaleWhaleWhale = TemporaryWhale;
            }
            catch (FormatException)
            {
                WhaliureCheck = true;
            }
            if (WhaliureCheck)
            {
                yield return "sendtochaterror Command format is not correct. Please fix this.";
                WhaliureCheck = false;
                yield break;
            }
            else if (WhaleWhaleWhale > 4)
            {
                yield return "sendtochaterror There are only four buttons, you can't press that.";
                yield break;
            }
            else if (WhaleWhaleWhale < 1)
            {
                yield return "sendtochaterror Too low. Try again.";
                yield break;
            }
            else {
            yield return null;
                if (MyWhale == WhaleWhaleWhale - 1)
                    yield return "solve";
                else
                    yield return "strike";
            whales[WhaleWhaleWhale - 1].OnInteract();
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve () {
        yield return new WaitForSeconds(0.1f);
        whales[MyWhale].OnInteract();
    }
    /*/
}
