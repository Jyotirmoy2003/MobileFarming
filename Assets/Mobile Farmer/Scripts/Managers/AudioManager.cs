using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public float audioVolumeFactor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if(s.Is3D) continue;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume*audioVolumeFactor;;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend=s.spatialBlend;
            if(s.PlayonAwake) s.source.Play();

        }
        
    }
    

   public void PlaySound(string name)
    {
       Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null){ print("value not found:"+name); return;}
        //if sound had to fade
        if(s.Fade)
        {
            s.source.volume=0;
            s.source?.Play();
            StartCoroutine(FadeSound(s,0.1f));
            return;
        }
        s.source?.Play();
        s.source.volume=s.volume*audioVolumeFactor;

    } 

    public void PlayBackgroundSound(string name,float timmer)
    {
       Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null){ print("value not found:"+name); return;}
        //if sound is already playing
        if(s.source.isPlaying)
        {
            //restart the coroutine
            StopCoroutine(StopSoundAfter(timmer,s));
            StartCoroutine(StopSoundAfter(timmer,s));
            //return
            return; 
        }
        
        //if sound had to fade
        if(s.Fade)
        {
            s.source.volume=0;
            s.source?.Play();
            StartCoroutine(FadeSound(s,0.1f));
            return;
        }
        
        s.source.Play();
        s.source.volume=s.volume*audioVolumeFactor;

    }

     

    //for 3d Objects
    public void PlaySound(string name,GameObject obj)
    {
       Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null){ return;}

        if(!obj.TryGetComponent<AudioSource>(out AudioSource so))
        {
            s.source = obj.AddComponent<AudioSource>();
            
        }else{
            s.source = obj.GetComponent<AudioSource>();
        }
        s.source.clip = s.clip;
        s.source.volume = s.volume*audioVolumeFactor;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend=s.spatialBlend;


        s.source?.Play();

    }
    public void StopAllSounds()
    {
        foreach(Sound item in sounds)
        {
            if(item.source==null) continue;
            item.source.Stop();
        }
    }

    public void StopSound(string name)
    {
        Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null){ return;}
        if(s.Fade)
        {
            StartCoroutine(FadeSound(s,-0.1f));
            return;
        }
        s.source?.Stop();
    }
     public void StopSound(string name,GameObject obj)
    {
        Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null){ return;}
         if(obj.TryGetComponent<AudioSource>(out AudioSource so))
        {
           so?.Stop();
        }
    }

    //fade effect
    IEnumerator FadeSound(Sound sound,float amount)
    {
        float vol=sound.volume*audioVolumeFactor;
        if(amount<0)
        {
            while(sound.source.volume>0)
            {
                sound.source.volume+=amount;
                yield return null;
            }
            sound.source.Stop();
        }else{
            while(sound.source.volume<=vol)
            {
                sound.source.volume+=amount;
                yield return null;
            }
        }
    }

    IEnumerator StopSoundAfter(float amount,Sound s )
    {
        yield return new WaitForSeconds(amount);
        StartCoroutine(FadeSound(s,s.volume*audioVolumeFactor));
    }

    //set sound level
    public void SetAudioVolumeFactor(float amount)
    {
        audioVolumeFactor=amount/100;
        foreach (Sound item in sounds)
        {
            try{
            if(item.source.isPlaying)
            {
                item.source.volume=item.volume*audioVolumeFactor;
            }}
            catch{
            }

        }
    }


}
