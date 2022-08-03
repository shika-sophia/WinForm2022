/** <!--
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT08_Resource
 *@class MainSoundPlayerWav.cs
 *@class   └ new FormSoundPlayerWav() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/resources/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm08_Resource.txt〕
 *           
 *@content   KT08 Resource / SoundPlayer
 *@reference 音声データのFree素材
 *           ■ Arky /Free SoundEffect / 過去のダウンロード
 *           http://www.arky.co.jp/schoollife_web/free_soundeffect/oldlog/0001_0020.html
 *          「0343.wav.zip」-> 「0343.wav」-> (Renamed) -> 「cannonSound.wav」
 *
 *@subject 「.wav」ファイル / WAV: Waveform Audio File Format
 *          https://e-words.jp/w/WAV.html
 *          ・WAVE形式 / WAVフォーマット
 *          ・WAVとは、音声データを記録するためのファイル形式の一つ。
 *          ・Windowsが標準で対応している形式拡張子は「.wav」
 *          ・汎用のデータ記録用ファイル形式であるRIFF（Resource Interchange File Format）形式を元に
 *            Microsoft社とIBM社が共同開発したファイル形式で、
 *            音声信号をデジタルデータ化したものをファイルに記録する形式を定めている。
 *            データのファイル内での配置や格納方式のみを定めたコンテナフォーマットの一つであり、
 *            データそのものの表現形式や圧縮形式は問わず、様々な圧縮形式のデータを格納できる。
 *          ・標準では無圧縮のPCM方式のデータが記録されていることが多いため
 *            無圧縮の音声フォーマットであると説明されることもあるがこれは誤解であり、
 *            実際、WMA形式や MP3形式などで圧縮された音声データをWAVファイルに記録することもできる。
 *          ・標準で対応していない形式の音声を記録・再生するためには、
 *            その形式を扱うためのコーデック（CODEC：COmpressor/DECompressor）が必要となる。
 *
 *@subject ◆SoundPlayer : Component, ISerializable  -- System.Media
 *         + SoundPlayer    new SoundPlayer()
 *         + SoundPlayer    new SoundPlayer(string soundLocation)
 *         + SoundPlayer    new SoundPlayer(Stream stream)
 *         # SoundPlayer    new SoundPlayer(SerializationInfo, StreamingContext)  //protected
 *         
 *         string     soundPlayer.SoundLocation  「.wav」ファイルの path, URL
 *         Stream     soundPlayer.Stream          ファイル読込のための System.IO.Stream
 *                      └ UnmanagedMemoryStream : Stream    「Resources.resx」で読み込んだ「.wav」の戻り値
 *                        例: player.Stream = WinFormGUI.Properties.Resources.cannonSound;
 *                        
 *         int        soundPlayer.LoadTimeout     タイムアウトを指定。ミリ秒単位
 *         bool       soundPlayer.IsLoadCompleted { get; }
 *         
 *         void       soundPlayer.Load()         Stream, Webから「.wav」ファイルを現在Threadで同期的に読込
 *         void       soundPlayer.LoadAsync()    新Threadを起動し非同期読込
 *         void       soundPlayer.Play()         新Threadを起動し、音声データを再生。読込まれていないときは読込
 *         void       soundPlayer.PlayLooping()  新Threadを起動し、反復再生。読込まれていないときは読込
 *         void       soundPlayer.PlaySync()     現在Threadで同期的に再生。読込まれていないときは読込
 *         void       soundPlayer.Stop()         音声データの再生を中断
 *         
 *         AsyncCompletedEventHandler LoadCompleted  非同期Load完了時のイベント
 *         
 *@subject WinFormGUI.Properties.Resources.resx 内
 *         player.Stream = WinFormGUI.Properties.Resources.cannonSound;
 *         
 *   <assembly alias="System.Windows.Forms" name="System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
 *   <data name="cannonSound" type="System.Resources.ResXFileRef, System.Windows.Forms">
 *       <value>..\datafile\soundwav\cannonsound.wav;System.IO.MemoryStream, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
 *   </data>
 *   
 *@see ImageSoundPlayerWav.jpg
 *@see ~\DataFile\SoundWav\dataStructureAboutWav.txt
 *@author shika
 *@date 2022-08-03
 * -->
 */
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT08_Resource
{
    class MainSoundPlayerWav
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormSoundPlayerWav()");

            Application.EnableVisualStyles();
            Application.Run(new FormSoundPlayerWav());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSoundPlayerWav : Form
    {
        private Label label;
        private Button btnPlay;
        private Button btnStop;
        private SoundPlayer player;

        public FormSoundPlayerWav()
        {
            this.Text = "FormSoundPlayerWav";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- SoundPlayer ----
            player = new SoundPlayer();
            player.SoundLocation = "../../DataFile/SoundWav/cannonSound.wav";
            //player.Stream = WinFormGUI.Properties.Resources.cannonSound;
            player.Load();

            string fileName = "";
            if (!String.IsNullOrEmpty(player.SoundLocation))
            {
                fileName = player.SoundLocation
                    .Substring(player.SoundLocation.LastIndexOf("/"))
                    .Replace("/", "");
            }

            //---- Controls ----
            label = new Label()
            {
                Text = fileName,
                Location = new Point(10, 10),
                Width = 200,
                BorderStyle = BorderStyle.FixedSingle,
            };

            btnPlay = new Button()
            {
                Text = "Play (&P)",
                Location = new Point(10, 50),
                AutoSize = true,
            };
            btnPlay.Click += new EventHandler(btnPlay_Click);
            this.AcceptButton = btnPlay;

            btnStop = new Button()
            {
                Text = "Stop (&S)",
                Location = new Point(120, 50),
                AutoSize = true,
            };
            btnStop.Click += new EventHandler(btnStop_Click);
            this.CancelButton = btnStop;

            this.Controls.AddRange(new Control[]
            {
                label, btnPlay, btnStop, 
            });
        }//constructor

        private void btnPlay_Click(object sender, EventArgs e)
        {
            player.PlayLooping();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.Stop();
        }
    }//class
}
