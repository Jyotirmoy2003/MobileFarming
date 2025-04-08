using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoSingleton<TransactionEffectManager>
{
    [Header("Coin")]
   [SerializeField] ParticleSystem cointParticel;
   [SerializeField] RectTransform cointRectTransform;
   [SerializeField] int cointParticelAmount=20;
   [SerializeField] float moveSpeed=5f;

   [Space]
   [Header("Gem")]
   [SerializeField] ParticleSystem gemParticel;
   [SerializeField] RectTransform gemRectTransform;
   [SerializeField] int gemParticelAmount=20;

   private Camera cameraMain;
   private int cashAmount = 0;


    void Start()
    {
        cameraMain = Camera.main;
    }
    [NaughtyAttributes.Button]
   private void PlayPrticelTest()
   {
        PlayGemParticel(cointParticelAmount);
   }

    [NaughtyAttributes.Button]
   private void PlayPrticeCoinTest()
   {
        PlayCoinParticel(cointParticelAmount);
   }

   public void PlayCoinParticel(int amount)
   {
        if(cointParticel.isPlaying) return;
        cashAmount =  amount;

        if(amount > 1500) amount = 1500; //cap particel amount


        ParticleSystem.Burst burst = cointParticel.emission.GetBurst(0);
        burst.count = amount;
        cointParticel.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main=cointParticel.main;
        main.gravityModifier = 2;

        cointParticelAmount = amount;
        cointParticel.Play();

        StartCoroutine (PlayCoinPartic1esCoroutine ( ) ) ;
   }

   public void PlayGemParticel(int amount)
   {
        if(gemParticel.isPlaying) return;
       


        ParticleSystem.Burst burst = gemParticel.emission.GetBurst(0);
        burst.count = amount;
        gemParticel.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main=gemParticel.main;
        main.gravityModifier = 2;

        
        gemParticel.Play();

        StartCoroutine (PlayGemParticlesCoroutine ( amount) ) ;
   }

   public void PlayGemParticel(int amount,Vector3 wordPos)
   {
        if(gemParticel.isPlaying) return;

        gemParticel.transform.position = wordPos;
        PlayGemParticel(amount);
   }

   IEnumerator PlayCoinPartic1esCoroutine()
   {
        yield return new WaitForSeconds(1);
        ParticleSystem.MainModule main=cointParticel.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles=new ParticleSystem.Particle[cointParticelAmount];
        cointParticel.GetParticles(particles);

        //calculate the path
        Vector3 direction = ( cointRectTransform.position-cameraMain.transform.position).normalized;
        
        while(cointParticel.isPlaying)
        {
            //calculate target in every frame so that when player moves it still finds its position
            Vector3 targetPosition = cameraMain.transform.position + direction * (Vector3.Distance(cameraMain.transform.position,cointParticel.transform.position));
            cointParticel.GetParticles(particles);
            for (int i =0; i < particles. Length; i++)
            {
                //when a particel already reached to the UI
                if(particles[i].remainingLifetime<=0) continue;

                particles[i].position = Vector3.MoveTowards(particles[i].position,targetPosition,moveSpeed * Time.deltaTime);

                //when particel reaches UI credite coin amount and remove that particel
                if(Vector3.Distance(particles[i].position,targetPosition)<0.1f)
                {
                    particles[i].position+=Vector3.up * 100000;
                    CashManager.Instance.CreditCoins(1);
                }
            }
            cointParticel.SetParticles(particles);

            yield return null;
        }

        if(cashAmount > 1500)
        {
            CashManager.Instance.CreditCoins(cashAmount-1500);
        }

   }









    IEnumerator PlayGemParticlesCoroutine(int particelAmount)
   {
        yield return new WaitForSeconds(1);
        ParticleSystem.MainModule main=gemParticel.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles=new ParticleSystem.Particle[particelAmount];
        gemParticel.GetParticles(particles);

        //calculate the path
        Vector3 direction = ( gemRectTransform.position-cameraMain.transform.position).normalized;
        
        while(gemParticel.isPlaying)
        {
            //calculate target in every frame so that when player moves it still finds its position
            Vector3 targetPosition = cameraMain.transform.position + direction * (Vector3.Distance(cameraMain.transform.position,gemParticel.transform.position));
            gemParticel.GetParticles(particles);
            for (int i =0; i < particles. Length; i++)
            {
                //when a particel already reached to the UI
                if(particles[i].remainingLifetime<=0) continue;

                particles[i].position = Vector3.MoveTowards(particles[i].position,targetPosition,moveSpeed * Time.deltaTime);

                //when particel reaches UI credite coin amount and remove that particel
                if(Vector3.Distance(particles[i].position,targetPosition)<0.1f)
                {
                    particles[i].position+=Vector3.up * 100000;
                    CashManager.Instance.CreditGems(1);
                }
            }
            gemParticel.SetParticles(particles);

            yield return null;
        }

       

   }
}
