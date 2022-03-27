using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsoundUnityHRTF : MonoBehaviour
{

    public float speed = 0.5f;

    private CsoundUnity csound;

    public GameObject localPlayer;

    public GameObject curCub;

    public CubeController cub;

    public int curNoteNumber;

    public float azimuth;
    public float elevation;

    public float azimuthPrev;
    public float elevationPrev;

    static float t = 0.0f;

    private int increment;

    private IEnumerator coroutine;

    public int[] noteQuantize = { 60, 62, 64, 67, 69, 71 };
    public int[] randOctave = { 0, -12, 7, -7 };

    // Start is called before the first frame update
    void Start()
    { 

        csound = GetComponent<CsoundUnity>();

        csound.SetStringChannel("CsoundFiles", Application.dataPath + "/CsoundFiles");
        print("CsoundFiles" + Application.dataPath + "/CsoundFiles");

        StartCoroutine(NoteIncrement());

        csound.SendScoreEvent($"i107 0 15000 {azimuth} {elevation}");
    }

    private void Update()
    {
        
        t += 1.0f * Time.deltaTime;


        if (t > 2.0f)
        {
            float temp = azimuthPrev;
            azimuthPrev = azimuth;
            azimuth = temp;
            t = 0.0f;
        }

        csound.SetChannel("azimuth", azimuth);
        csound.SetChannel("elevation", elevation);

        azimuthPrev = azimuth;
        elevationPrev = elevation;

    }

    IEnumerator NoteIncrement()
    {
        
        while (true)
        {
            print(noteToFreq(curNoteNumber));

            if (increment >= 24)
            {
                increment = 0;
            }

            curCub = cub.cubes[increment];

            var cubeRenderer = curCub.GetComponent<Renderer>();

            curNoteNumber = curCub.GetComponent<OscillatorController>().noteNumber;

            azimuth = Mathf.Lerp(azimuthPrev, AzimuthAngle(curCub, localPlayer), t);

            elevation = Mathf.Lerp(elevationPrev, ElevationAngle(curCub, localPlayer), t);

            float volume = Distance(curCub, localPlayer) * 8;

            csound.SendScoreEvent($"i1 0 0.5 {noteToFreq(noteQuantize[Random.Range(0, 5)] + randOctave[Random.Range(0, 3)])}");

            cubeRenderer.material.color = Random.ColorHSV();

            increment++;

            yield return new WaitForSeconds(speed);
        }
        

    }

    float AzimuthAngle(GameObject cube, GameObject play)
    {
        float azi = Vector3.Angle(play.transform.forward, cube.transform.position - play.transform.position);

        var cross = Vector3.Cross(play.transform.forward, cube.transform.position - play.transform.position);

        if (cross.y < 0)
        {
            azi = -azi;
        }

        return azi;
    }

    float ElevationAngle(GameObject cube, GameObject play)
    {
        float elev = Vector3.Angle(play.transform.up, cube.transform.position - play.transform.position);

        var cross2 = Vector3.Cross(play.transform.up, cube.transform.position - play.transform.position);

        if (cross2.x < 0)
        {
            elev = -elev;
        }

        return elev;
    }

    float Distance(GameObject cube, GameObject play)
    {
        float dist = Vector3.Distance(cube.transform.position, play.transform.position);

        if (dist < 1)
        {
            dist = 1;
        }

        float volumDist = 1 / dist;

        float volum = Mathf.Pow(volumDist, 1f);

        return volum;

    }

    public float noteToFreq(int note)
    {
        float a = 440;
        return (a) * Mathf.Pow(2, (float)((note - 69) / 12.0));
    }
}
