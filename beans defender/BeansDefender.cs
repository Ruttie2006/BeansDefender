using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using Modding;
using UnityEngine;

namespace BeansDefender
{
    
    public class BeansDefender : Mod
    {
        public override string GetVersion()
        {
            return "1.2";
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Waterways_05_boss", "Dung Defender")
            };
        }

        public GameObject bd;
        public IEnumerator ReplaceDungDefender()
        {
            yield return new WaitWhile(() => HeroController.instance == null);
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string res in asm.GetManifestResourceNames())
            {   
                if(!res.EndsWith("DungDefender.png")) {
                    continue;
                } 
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                        if (s == null) continue;
                        var buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                        var texture2 = new Texture2D(2, 2);
                        texture2.LoadImage(buffer.ToArray(),true);
                        bd.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = texture2;
                        s.Dispose();
                        
                }
            }
           
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("started initialize");
            bd = preloadedObjects["Waterways_05_boss"]["Dung Defender"];
            ModHooks.Instance.LanguageGetHook += Beans;
            GameManager.instance.StartCoroutine(ReplaceDungDefender());
            Log("Initialize done");
        }

        private string Beans(string key, string sheetTitle)
        {
            //Log("Languagegethook called");
            if (key == "DUNG_DEFENDER_MAIN")
            {
                //Log("Dung_Defender_Main found");
                return "Me When";
            }
            if (key == "DUNG_DEFENDER_SUB")
            {
                //Log("Dung_Defender_Sub found");
                return "Beans";
            }
            /*Log("Key:");
            Log(key);
            Log("");
            Log("sheetTitle:");
            Log(sheetTitle);*/
            return Language.Language.GetInternal(key, sheetTitle);
        }

        /*public string LanguageGet(string key, string sheet)
        {
            if (key == "DUNG_DEFENDER_MAIN")
            {
                return "Me When";
            }
            if (key == "DUNG_DEFENDER_SUB")
            {
                return "BEANS";
            }
            return Language.Language.GetInternal(key, sheet);
        }*/
    }
}
