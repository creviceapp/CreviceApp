﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crevice.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Crevice.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   (アイコン) に類似した型 System.Drawing.Icon のローカライズされたリソースを検索します。
        /// </summary>
        internal static System.Drawing.Icon CreviceIcon {
            get {
                object obj = ResourceManager.GetObject("CreviceIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   型 System.Byte[] のローカライズされたリソースを検索します。
        /// </summary>
        internal static byte[] DefaultUserScript {
            get {
                object obj = ResourceManager.GetObject("DefaultUserScript", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   &lt;html&gt;
        ///&lt;head&gt;
        ///&lt;style type=&quot;text/css&quot;&gt;
        ///html 
        ///{{
        /// 	line-height: 1.8; 
        ///}}
        ///.appname 
        ///{{
        /// 	font-size: 100%; 
        /// 	font-weight: bold; 
        ///}}
        ///.version 
        ///{{
        /// 	font-size: 100%; 
        ///}}
        ///.license 
        ///{{
        /// 	font-size: 80%; 
        ///}}  
        ///.link 
        ///{{
        /// 	font-size: 80%; 
        ///}}  
        ///.usage
        ///{{
        /// 	font-size: 60%; 
        ///}}
        ///.center 
        ///{{
        /// 	text-align: center; 
        ///}}
        ///&lt;/style&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///    &lt;div class=&quot;center&quot;&gt;&lt;span class=&quot;appname&quot;&gt;{0}&lt;/span&gt; &lt;span class=&quot;version&quot;&gt;{1}&lt;/span&gt;&lt;/div&gt;
        ///    &lt;div class=&quot;center license&quot;&gt;{2}&lt;/div&gt;
        ///    &lt;di [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ProductInfo {
            get {
                return ResourceManager.GetString("ProductInfo", resourceCulture);
            }
        }
    }
}
