using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniGLTF.Utils;
using UnityEditor;

namespace UniGLTF.M17N
{
    /// <summary>
    /// 多言語対応
    /// </summary>
    public enum Languages
    {
        ja,
        en,
        ko,
        zh_HANS,
        zh_HANT,
        es,
        fr,
        de,
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class LangMsgAttribute : System.Attribute
    {
        public Languages Language;
        public string Message;

        public LangMsgAttribute(Languages language, string msg)
        {
            Language = language;
            Message = msg;
        }
    }

    public static class MsgCache<T> where T : struct, Enum
    {
        static Dictionary<Languages, Dictionary<T, string>> s_cache = new Dictionary<Languages, Dictionary<T, string>>();

        static LangMsgAttribute GetAttribute(T value, Languages language)
        {
            var t = typeof(T);
            var memberInfos = t.GetMember(value.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == t);
            var attr = enumValueMemberInfo.GetCustomAttributes(typeof(LangMsgAttribute), false).Select(x => (LangMsgAttribute)x).ToArray();
            if (attr == null || attr.Length == 0)
            {
                return null;
            }
            var match = attr.FirstOrDefault(x => x.Language == language);
            if (match != null)
            {
                return match;
            }
            return attr.FirstOrDefault(x => x.Language == Languages.en) ?? attr.First();
        }

        public static string Get(Languages language, T key)
        {
            if (!s_cache.TryGetValue(language, out Dictionary<T, string> map))
            {
                map = new Dictionary<T, string>();

                var t = typeof(T);
                foreach (T value in CachedEnum.GetValues<T>())
                {
                    var match = GetAttribute(value, language);
                    // Attribute。無かったら enum の ToString
                    map.Add(value, match != null ? match.Message : value.ToString());
                }

                s_cache.Add(language, map);
            }
            return map[key];
        }
    }
    public static class LanguageGetter
    {
        const string LANG_KEY = "VRM_LANG";

        // Unity 2022.3 stores the Editor language separately from the OS.
        // Reading it leaves the user's Editor and UniVRM preferences untouched.
        internal static Languages EditorLanguage()
        {
            if (!EditorPrefs.GetBool("Editor.kEnableEditorLocalization", true))
                return Languages.en;
            var language = Application.systemLanguage;
            if (Enum.TryParse<SystemLanguage>(EditorPrefs.GetString("Editor.kEditorLocale"), true, out var selected))
                language = selected;
            switch (language)
            {
                case SystemLanguage.Japanese: return Languages.ja;
                case SystemLanguage.Korean: return Languages.ko;
                case SystemLanguage.ChineseTraditional: return Languages.zh_HANT;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified: return Languages.zh_HANS;
                case SystemLanguage.Spanish: return Languages.es;
                case SystemLanguage.French: return Languages.fr;
                case SystemLanguage.German: return Languages.de;
                default: return Languages.en;
            }
        }

        static Languages? s_lang;

        public static Languages Lang
        {
            get
            {
                if (!s_lang.HasValue)
                {
                    var value = EditorPrefs.GetString(LANG_KEY);
                    if (!string.IsNullOrEmpty(value) && Enum.TryParse<Languages>(value, true, out Languages parsed) && Enum.IsDefined(typeof(Languages), parsed))
                    {
                        s_lang = parsed;
                    }
                    else
                    {
                        s_lang = EditorLanguage();
                    }
                }
                return s_lang.GetValueOrDefault();
            }
        }

        public static string Msg<T>(this T key) where T : struct, Enum
        {
            return MsgCache<T>.Get(Lang, key);
        }

        public static void OnGuiSelectLang()
        {
            var lang = (Languages)EditorGUILayout.EnumPopup("lang", Lang);
            if (lang != Lang)
            {
                s_lang = lang;
                EditorPrefs.SetString(LANG_KEY, LanguageGetter.Lang.ToString());
            }
        }
    }
}
