namespace MultiCombat
{
    using MultiCombat.Classes;
    using MultiCombat.Forms;
    using MyTERA.Helpers.SkillsManager;
    using MyTERA.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using ZurasBot.Classes;
    using ZurasBot.Functions;

    public class Settings : Form
    {
        private Button btnCast;
        private Button btnClear;
        private Button btnLoad;
        private Button btnOK;
        private Button btnRotation;
        private ListBox buffList;
        private Button Cancel;
        private CastTime Cast;
        private NumericUpDown chanceCharging;
        private NumericUpDown chanceDefault;
        private ListBox CombatSkillList;
        private ListBox Combo;
        private IContainer components;
        private NumericUpDown CRDistance;
        private ListBox Double;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private ListBox HealList;
        private CheckBox isBandage;
        private CheckBox isDetectAggro;
        private CheckBox isDetectPlayer;
        private CheckBox isHPItem;
        private CheckBox isLogCast;
        private CheckBox isLogChain;
        private CheckBox isLogXP;
        private CheckBox isLootOnlyGold;
        private CheckBox isMCLoot;
        private CheckBox isMPItem;
        private CheckBox isStamina;
        private CheckBox isStrafeEnabled;
        private CheckBox isStrafeOnCharge;
        private CheckBox isStrafePlayer;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListBox lernedSkill;
        private ListBox LongRange;
        private CheckBox lootCommon;
        private CheckBox lootEpic;
        private CheckBox lootSuperior;
        private CheckBox lootUncommon;
        private NumericUpDown LRDistance;
        private ListBox ManaMana;
        private NumericUpDown maxAggroDist;
        private NumericUpDown maxLootDistance;
        private NumericUpDown maxPlayerDist;
        private NumericUpDown maxStrafeMs;
        private NumericUpDown MinHpPerc;
        private NumericUpDown MinMpPerc;
        private NumericUpDown minStamina;
        private NumericUpDown minStrafeMs;
        private ListBox NoChain;
        private Button OK;
        private Player plr = new Player();
        private NumericUpDown PullDistance;
        private ListBox PullSkillList;
        private ComboManager Rotation;
        private TabControl Skills;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TabPage tabPage8;
        private CheckBox useCampfire;
        private CheckBox usePanacea;
        private NumericUpDown waitMS;
        private CheckBox waitMSEnabler;
        private ListBox waitSkillList;

        public Settings()
        {
            this.InitializeComponent();
            this.LoadSettingsData();
            this.RefreshSettings();
            if (this.Cast != null)
            {
                this.Cast.Close();
                this.Cast.Dispose();
            }
            this.Cast = new CastTime();
        }

        private void btnCast_Click(object sender, EventArgs e)
        {
            if (this.Cast != null)
            {
                this.Cast.Close();
                this.Cast.Dispose();
            }
            this.Cast = new CastTime();
            this.Cast.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearSettings();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                FileName = string.Concat(new object[] { "Settings_", Player.ClassName, "_", Player.GetLevel() }),
                Title = "Load combat profile ...",
                Filter = "MultiCombat Profile (*.xml)|*.xml",
                DefaultExt = "xml",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Globals.Settings = XmlSerializer.Deserialize<MultiCombat.Classes.Settings>(dialog.FileName);
                new IniFile(Application.StartupPath + @"\MC_Settings.ini").IniWriteValue("MultiCombat", "LastProfile", dialog.FileName);
            }
            this.Settings_Load(this, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.RefreshSettings();
            base.Close();
        }

        private void btnRotation_Click(object sender, EventArgs e)
        {
            if (this.Rotation != null)
            {
                this.Rotation.Close();
                this.Rotation.Dispose();
            }
            this.Rotation = new ComboManager();
            this.Rotation.Show();
        }

        private void buffList_DoubleClick_1(object sender, EventArgs e)
        {
            this.buffList.Items.Remove(this.buffList.SelectedItem);
        }

        private void buffList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.buffList.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void buffList_DragOver(object sender, DragEventArgs e)
        {
            if (this.buffList.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void chainSkillList_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.waitSkillList.Items.Count != 0)
            {
                this.waitSkillList.DoDragDrop(this.waitSkillList.SelectedItem.ToString(), DragDropEffects.Copy);
            }
        }

        private void ClearSettings()
        {
            this.LoadLearnedList();
            this.PullSkillList.Items.Clear();
            this.CombatSkillList.Items.Clear();
            this.Combo.Items.Clear();
            this.ManaMana.Items.Clear();
            this.Double.Items.Clear();
            this.NoChain.Items.Clear();
            this.LongRange.Items.Clear();
            this.HealList.Items.Clear();
            this.waitSkillList.Items.Clear();
            this.buffList.Items.Clear();
            this.waitMSEnabler.Checked = false;
            this.isLogCast.Checked = true;
            this.isLogChain.Checked = true;
            this.isLogXP.Checked = true;
            this.waitMS.Value = 1000M;
            this.LRDistance.Value = 10M;
            this.CRDistance.Value = 4M;
            this.PullDistance.Value = 10M;
            this.MinMpPerc.Value = 40M;
            this.MinHpPerc.Value = 40M;
            this.isMCLoot.Checked = true;
            this.isLootOnlyGold.Checked = false;
            this.maxLootDistance.Value = 30M;
            Globals.Settings.castTimeSkills.Clear();
            this.isDetectPlayer.Checked = true;
            this.maxPlayerDist.Value = 500M;
            this.isDetectAggro.Checked = true;
            this.maxAggroDist.Value = 10M;
            this.isStrafeEnabled.Checked = false;
            this.isStamina.Checked = true;
            this.useCampfire.Checked = true;
            this.usePanacea.Checked = true;
            this.minStamina.Value = 20M;
            this.isBandage.Checked = true;
            this.isMPItem.Checked = true;
            this.isHPItem.Checked = true;
            this.isStrafeOnCharge.Checked = true;
            this.isStrafePlayer.Checked = true;
            this.minStrafeMs.Value = 600M;
            this.maxStrafeMs.Value = 800M;
            this.chanceDefault.Value = 20M;
            this.chanceCharging.Value = 90M;
            this.lootCommon.Checked = true;
            this.lootEpic.Checked = true;
            this.lootSuperior.Checked = true;
            this.lootUncommon.Checked = true;
        }

        private void CombatSkillList_DoubleClick(object sender, EventArgs e)
        {
            this.CombatSkillList.Items.Remove(this.CombatSkillList.SelectedItem);
        }

        private void CombatSkillList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.CombatSkillList.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void CombatSkillList_DragOver(object sender, DragEventArgs e)
        {
            if (this.CombatSkillList.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void Combo_DoubleClick(object sender, EventArgs e)
        {
            this.Combo.Items.Remove(this.Combo.SelectedItem);
        }

        private void Combo_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.Combo.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void Combo_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
        }

        public Skill ConvertToSkill(object item)
        {
            Structs.TERASkill tERASkillByDataName = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillByDataName(item.ToString());
            return new Skill { SkillId = tERASkillByDataName.Id, Name = tERASkillByDataName.Name, CastTimeSeconds = 0, DoubleAction = false, MaxRange = 0, MinRange = 0 };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Double_DoubleClick(object sender, EventArgs e)
        {
            this.Double.Items.Remove(this.Double.SelectedItem);
        }

        private void Double_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.Double.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void Double_DragOver(object sender, DragEventArgs e)
        {
            if (this.Double.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void HealList_DoubleClick(object sender, EventArgs e)
        {
            this.HealList.Items.Remove(this.HealList.SelectedItem);
        }

        private void HealList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.HealList.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void HealList_DragOver(object sender, DragEventArgs e)
        {
            if (this.HealList.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void InitializeComponent()
        {
            this.OK = new Button();
            this.Cancel = new Button();
            this.tabPage4 = new TabPage();
            this.btnRotation = new Button();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.Double = new ListBox();
            this.ManaMana = new ListBox();
            this.Combo = new ListBox();
            this.label4 = new Label();
            this.NoChain = new ListBox();
            this.tabPage3 = new TabPage();
            this.label15 = new Label();
            this.label14 = new Label();
            this.buffList = new ListBox();
            this.HealList = new ListBox();
            this.tabPage2 = new TabPage();
            this.LRDistance = new NumericUpDown();
            this.CRDistance = new NumericUpDown();
            this.label7 = new Label();
            this.label6 = new Label();
            this.LongRange = new ListBox();
            this.CombatSkillList = new ListBox();
            this.tabPage1 = new TabPage();
            this.label8 = new Label();
            this.PullDistance = new NumericUpDown();
            this.PullSkillList = new ListBox();
            this.lernedSkill = new ListBox();
            this.Skills = new TabControl();
            this.tabPage7 = new TabPage();
            this.label12 = new Label();
            this.waitMS = new NumericUpDown();
            this.waitMSEnabler = new CheckBox();
            this.label11 = new Label();
            this.waitSkillList = new ListBox();
            this.btnLoad = new Button();
            this.label5 = new Label();
            this.tabControl1 = new TabControl();
            this.tabPage5 = new TabPage();
            this.btnCast = new Button();
            this.tabPage6 = new TabPage();
            this.groupBox5 = new GroupBox();
            this.label19 = new Label();
            this.chanceCharging = new NumericUpDown();
            this.label18 = new Label();
            this.chanceDefault = new NumericUpDown();
            this.label17 = new Label();
            this.label16 = new Label();
            this.maxStrafeMs = new NumericUpDown();
            this.minStrafeMs = new NumericUpDown();
            this.isStrafeOnCharge = new CheckBox();
            this.isStrafePlayer = new CheckBox();
            this.isStrafeEnabled = new CheckBox();
            this.groupBox4 = new GroupBox();
            this.maxAggroDist = new NumericUpDown();
            this.maxPlayerDist = new NumericUpDown();
            this.isDetectAggro = new CheckBox();
            this.isDetectPlayer = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.isLogCast = new CheckBox();
            this.isLogChain = new CheckBox();
            this.isLogXP = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.isBandage = new CheckBox();
            this.isMPItem = new CheckBox();
            this.isHPItem = new CheckBox();
            this.usePanacea = new CheckBox();
            this.useCampfire = new CheckBox();
            this.minStamina = new NumericUpDown();
            this.isStamina = new CheckBox();
            this.MinHpPerc = new NumericUpDown();
            this.MinMpPerc = new NumericUpDown();
            this.label10 = new Label();
            this.label9 = new Label();
            this.tabPage8 = new TabPage();
            this.groupBox3 = new GroupBox();
            this.lootEpic = new CheckBox();
            this.lootSuperior = new CheckBox();
            this.lootUncommon = new CheckBox();
            this.lootCommon = new CheckBox();
            this.isLootOnlyGold = new CheckBox();
            this.label13 = new Label();
            this.maxLootDistance = new NumericUpDown();
            this.isMCLoot = new CheckBox();
            this.btnOK = new Button();
            this.btnClear = new Button();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.LRDistance.BeginInit();
            this.CRDistance.BeginInit();
            this.tabPage1.SuspendLayout();
            this.PullDistance.BeginInit();
            this.Skills.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.waitMS.BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.chanceCharging.BeginInit();
            this.chanceDefault.BeginInit();
            this.maxStrafeMs.BeginInit();
            this.minStrafeMs.BeginInit();
            this.groupBox4.SuspendLayout();
            this.maxAggroDist.BeginInit();
            this.maxPlayerDist.BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.minStamina.BeginInit();
            this.MinHpPerc.BeginInit();
            this.MinMpPerc.BeginInit();
            this.tabPage8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.maxLootDistance.BeginInit();
            base.SuspendLayout();
            this.OK.AllowDrop = true;
            this.OK.Location = new Point(0x13, 0x164);
            this.OK.Name = "OK";
            this.OK.Size = new Size(0x4a, 0x21);
            this.OK.TabIndex = 0;
            this.OK.Text = "Save";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new EventHandler(this.OK_Click);
            this.Cancel.AccessibleName = "Cancel";
            this.Cancel.DialogResult = DialogResult.Cancel;
            this.Cancel.Location = new Point(430, 0x164);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new Size(0x4a, 0x21);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new EventHandler(this.Cancel_Click);
            this.tabPage4.Controls.Add(this.btnRotation);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.Double);
            this.tabPage4.Controls.Add(this.ManaMana);
            this.tabPage4.Controls.Add(this.Combo);
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new Padding(3);
            this.tabPage4.Size = new Size(0xea, 270);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Misc";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.btnRotation.Location = new Point(90, 0x61);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new Size(0x77, 0x16);
            this.btnRotation.TabIndex = 8;
            this.btnRotation.Text = "Rotation Manager";
            this.btnRotation.UseVisualStyleBackColor = true;
            this.btnRotation.Visible = false;
            this.btnRotation.Click += new EventHandler(this.btnRotation_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Combo";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 0x6d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Double Cast";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0xbc);
            this.label1.Name = "label1";
            this.label1.Size = new Size(60, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Regain MP";
            this.Double.AllowDrop = true;
            this.Double.FormattingEnabled = true;
            this.Double.Location = new Point(6, 0x7d);
            this.Double.Name = "Double";
            this.Double.Size = new Size(0xd4, 0x38);
            this.Double.TabIndex = 2;
            this.Double.DragDrop += new DragEventHandler(this.Double_DragDrop);
            this.Double.DragOver += new DragEventHandler(this.Double_DragOver);
            this.Double.DoubleClick += new EventHandler(this.Double_DoubleClick);
            this.ManaMana.AllowDrop = true;
            this.ManaMana.FormattingEnabled = true;
            this.ManaMana.Location = new Point(6, 0xcc);
            this.ManaMana.Name = "ManaMana";
            this.ManaMana.Size = new Size(0xd4, 0x38);
            this.ManaMana.TabIndex = 1;
            this.ManaMana.DragDrop += new DragEventHandler(this.ManaMana_DragDrop);
            this.ManaMana.DragOver += new DragEventHandler(this.ManaMana_DragOver);
            this.ManaMana.DoubleClick += new EventHandler(this.ManaMana_DoubleClick);
            this.Combo.AllowDrop = true;
            this.Combo.FormattingEnabled = true;
            this.Combo.Location = new Point(6, 0x10);
            this.Combo.Name = "Combo";
            this.Combo.Size = new Size(0xd4, 0x45);
            this.Combo.TabIndex = 0;
            this.Combo.DragDrop += new DragEventHandler(this.Combo_DragDrop);
            this.Combo.DragOver += new DragEventHandler(this.Combo_DragOver);
            this.Combo.DoubleClick += new EventHandler(this.Combo_DoubleClick);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(7, 3);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Don't chain:";
            this.NoChain.AllowDrop = true;
            this.NoChain.FormattingEnabled = true;
            this.NoChain.Location = new Point(10, 0x13);
            this.NoChain.Name = "NoChain";
            this.NoChain.Size = new Size(0xda, 0x45);
            this.NoChain.TabIndex = 3;
            this.NoChain.DragDrop += new DragEventHandler(this.NoChain_DragDrop);
            this.NoChain.DragOver += new DragEventHandler(this.NoChain_DragOver);
            this.NoChain.DoubleClick += new EventHandler(this.NoChain_DoubleClick);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.buffList);
            this.tabPage3.Controls.Add(this.HealList);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new Padding(3);
            this.tabPage3.Size = new Size(0xea, 270);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Heal";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x10, 120);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x1a, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Buff";
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x10, 12);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x1d, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Heal";
            this.buffList.AllowDrop = true;
            this.buffList.FormattingEnabled = true;
            this.buffList.Location = new Point(10, 0x88);
            this.buffList.Name = "buffList";
            this.buffList.Size = new Size(0xd3, 0x5f);
            this.buffList.TabIndex = 8;
            this.buffList.DragDrop += new DragEventHandler(this.buffList_DragDrop);
            this.buffList.DragOver += new DragEventHandler(this.buffList_DragOver);
            this.buffList.DoubleClick += new EventHandler(this.buffList_DoubleClick_1);
            this.HealList.AllowDrop = true;
            this.HealList.FormattingEnabled = true;
            this.HealList.Location = new Point(10, 0x1c);
            this.HealList.Name = "HealList";
            this.HealList.Size = new Size(0xd3, 0x52);
            this.HealList.TabIndex = 7;
            this.HealList.DragDrop += new DragEventHandler(this.HealList_DragDrop);
            this.HealList.DragOver += new DragEventHandler(this.HealList_DragOver);
            this.HealList.DoubleClick += new EventHandler(this.HealList_DoubleClick);
            this.tabPage2.Controls.Add(this.LRDistance);
            this.tabPage2.Controls.Add(this.CRDistance);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.LongRange);
            this.tabPage2.Controls.Add(this.CombatSkillList);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0xea, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Combat";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.LRDistance.Location = new Point(0xae, 0x89);
            this.LRDistance.Name = "LRDistance";
            this.LRDistance.Size = new Size(0x27, 20);
            this.LRDistance.TabIndex = 0x13;
            this.LRDistance.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 0x11;
            this.LRDistance.Value = new decimal(bits);
            this.CRDistance.Location = new Point(0xae, 3);
            this.CRDistance.Name = "CRDistance";
            this.CRDistance.Size = new Size(0x27, 20);
            this.CRDistance.TabIndex = 0x12;
            this.CRDistance.TextAlign = HorizontalAlignment.Right;
            int[] numArray2 = new int[4];
            numArray2[0] = 4;
            this.CRDistance.Value = new decimal(numArray2);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(6, 140);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x89, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Long Range Distance (Min)";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 7);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x8b, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Close Range Distance (Min)";
            this.LongRange.AllowDrop = true;
            this.LongRange.FormattingEnabled = true;
            this.LongRange.Location = new Point(6, 0x9c);
            this.LongRange.Name = "LongRange";
            this.LongRange.Size = new Size(0xd3, 0x6c);
            this.LongRange.TabIndex = 13;
            this.LongRange.DragDrop += new DragEventHandler(this.LongRange_DragDrop);
            this.LongRange.DragOver += new DragEventHandler(this.LongRange_DragOver);
            this.LongRange.DoubleClick += new EventHandler(this.LongRange_DoubleClick);
            this.CombatSkillList.AllowDrop = true;
            this.CombatSkillList.FormattingEnabled = true;
            this.CombatSkillList.Location = new Point(6, 0x17);
            this.CombatSkillList.Name = "CombatSkillList";
            this.CombatSkillList.Size = new Size(0xd3, 0x6c);
            this.CombatSkillList.TabIndex = 12;
            this.CombatSkillList.DragDrop += new DragEventHandler(this.CombatSkillList_DragDrop);
            this.CombatSkillList.DragOver += new DragEventHandler(this.CombatSkillList_DragOver);
            this.CombatSkillList.DoubleClick += new EventHandler(this.CombatSkillList_DoubleClick);
            this.tabPage1.AccessibleDescription = "Pull Skills";
            this.tabPage1.AccessibleName = "Pull Skills";
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.PullDistance);
            this.tabPage1.Controls.Add(this.PullSkillList);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0xea, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pull";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x4d, 0xeb);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x59, 13);
            this.label8.TabIndex = 0x12;
            this.label8.Text = "Pull Distance Min";
            this.PullDistance.Location = new Point(0xb9, 0xe9);
            this.PullDistance.Name = "PullDistance";
            this.PullDistance.Size = new Size(0x24, 20);
            this.PullDistance.TabIndex = 0x13;
            this.PullDistance.TextAlign = HorizontalAlignment.Right;
            int[] numArray3 = new int[4];
            numArray3[0] = 0x11;
            this.PullDistance.Value = new decimal(numArray3);
            this.PullSkillList.AccessibleDescription = "PullSkillList";
            this.PullSkillList.AccessibleName = "PullSkillList";
            this.PullSkillList.AllowDrop = true;
            this.PullSkillList.FormattingEnabled = true;
            this.PullSkillList.Location = new Point(10, 6);
            this.PullSkillList.Name = "PullSkillList";
            this.PullSkillList.Size = new Size(0xd3, 0xd4);
            this.PullSkillList.TabIndex = 12;
            this.PullSkillList.SelectedIndexChanged += new EventHandler(this.PullSkillList_SelectedIndexChanged);
            this.PullSkillList.DragDrop += new DragEventHandler(this.PullSkillList_DragDrop);
            this.PullSkillList.DragOver += new DragEventHandler(this.PullSkillList_DragOver);
            this.PullSkillList.DoubleClick += new EventHandler(this.PullSkillList_DoubleClick);
            this.lernedSkill.AllowDrop = true;
            this.lernedSkill.FormattingEnabled = true;
            this.lernedSkill.Location = new Point(11, 0x22);
            this.lernedSkill.Name = "lernedSkill";
            this.lernedSkill.Size = new Size(0xd3, 0xe1);
            this.lernedSkill.Sorted = true;
            this.lernedSkill.TabIndex = 0x10;
            this.lernedSkill.Tag = "LernedSkillPull";
            this.lernedSkill.SelectedIndexChanged += new EventHandler(this.lernedSkillPull_SelectedIndexChanged);
            this.lernedSkill.MouseDown += new MouseEventHandler(this.lernedSkillPull_MouseDown);
            this.Skills.AccessibleDescription = "Skills";
            this.Skills.AccessibleName = "Skills";
            this.Skills.Controls.Add(this.tabPage1);
            this.Skills.Controls.Add(this.tabPage2);
            this.Skills.Controls.Add(this.tabPage3);
            this.Skills.Controls.Add(this.tabPage7);
            this.Skills.Controls.Add(this.tabPage4);
            this.Skills.Location = new Point(0xf6, 6);
            this.Skills.Name = "Skills";
            this.Skills.SelectedIndex = 0;
            this.Skills.Size = new Size(0xf2, 0x128);
            this.Skills.TabIndex = 2;
            this.tabPage7.Controls.Add(this.label12);
            this.tabPage7.Controls.Add(this.waitMS);
            this.tabPage7.Controls.Add(this.waitMSEnabler);
            this.tabPage7.Controls.Add(this.label11);
            this.tabPage7.Controls.Add(this.waitSkillList);
            this.tabPage7.Controls.Add(this.label4);
            this.tabPage7.Controls.Add(this.NoChain);
            this.tabPage7.Location = new Point(4, 0x16);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new Padding(3);
            this.tabPage7.Size = new Size(0xea, 270);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "Chain";
            this.tabPage7.UseVisualStyleBackColor = true;
            this.label12.AutoSize = true;
            this.label12.Location = new Point(120, 0xc2);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x15, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Ms";
            this.waitMS.Location = new Point(70, 0xc0);
            int[] numArray4 = new int[4];
            numArray4[0] = 0x2328;
            this.waitMS.Maximum = new decimal(numArray4);
            this.waitMS.Name = "waitMS";
            this.waitMS.Size = new Size(0x2c, 20);
            this.waitMS.TabIndex = 12;
            this.waitMS.TextAlign = HorizontalAlignment.Right;
            int[] numArray5 = new int[4];
            numArray5[0] = 0x3e8;
            this.waitMS.Value = new decimal(numArray5);
            this.waitMSEnabler.AutoSize = true;
            this.waitMSEnabler.Location = new Point(10, 0xc1);
            this.waitMSEnabler.Name = "waitMSEnabler";
            this.waitMSEnabler.Size = new Size(0x40, 0x11);
            this.waitMSEnabler.TabIndex = 11;
            this.waitMSEnabler.Text = "for Max:";
            this.waitMSEnabler.UseVisualStyleBackColor = true;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(7, 0x5c);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x49, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Wait to chain:";
            this.waitSkillList.AllowDrop = true;
            this.waitSkillList.FormattingEnabled = true;
            this.waitSkillList.Location = new Point(10, 0x6c);
            this.waitSkillList.Name = "waitSkillList";
            this.waitSkillList.Size = new Size(0xda, 0x52);
            this.waitSkillList.TabIndex = 8;
            this.waitSkillList.DragDrop += new DragEventHandler(this.waitSkillList_DragDrop);
            this.waitSkillList.DragOver += new DragEventHandler(this.waitSkillList_DragOver);
            this.waitSkillList.DoubleClick += new EventHandler(this.waitSkillList_DoubleClick);
            this.btnLoad.Location = new Point(0x70, 0x164);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new Size(0x4a, 0x21);
            this.btnLoad.TabIndex = 0x11;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new EventHandler(this.btnLoad_Click);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(8, 0x12);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x44, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Learned Skill";
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Location = new Point(12, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x1f6, 0x158);
            this.tabControl1.TabIndex = 0x10;
            this.tabPage5.Controls.Add(this.btnCast);
            this.tabPage5.Controls.Add(this.Skills);
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.lernedSkill);
            this.tabPage5.Location = new Point(4, 0x16);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new Padding(3);
            this.tabPage5.Size = new Size(0x1ee, 0x13e);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Skill Manager";
            this.tabPage5.UseVisualStyleBackColor = true;
            this.btnCast.Location = new Point(11, 0x113);
            this.btnCast.Name = "btnCast";
            this.btnCast.Size = new Size(0x72, 0x17);
            this.btnCast.TabIndex = 0x11;
            this.btnCast.Text = "Cast Time Manager";
            this.btnCast.UseVisualStyleBackColor = true;
            this.btnCast.Click += new EventHandler(this.btnCast_Click);
            this.tabPage6.Controls.Add(this.groupBox5);
            this.tabPage6.Controls.Add(this.groupBox4);
            this.tabPage6.Controls.Add(this.groupBox2);
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.Location = new Point(4, 0x16);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new Padding(3);
            this.tabPage6.Size = new Size(0x1ee, 0x13e);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Combat Manager";
            this.tabPage6.UseVisualStyleBackColor = true;
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.chanceCharging);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.chanceDefault);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.maxStrafeMs);
            this.groupBox5.Controls.Add(this.minStrafeMs);
            this.groupBox5.Controls.Add(this.isStrafeOnCharge);
            this.groupBox5.Controls.Add(this.isStrafePlayer);
            this.groupBox5.Controls.Add(this.isStrafeEnabled);
            this.groupBox5.Location = new Point(6, 0xcc);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0xf2, 0x6c);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "In Combat Option";
            this.label19.AutoSize = true;
            this.label19.Location = new Point(0xcf, 0x3a);
            this.label19.Name = "label19";
            this.label19.Size = new Size(30, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "/100";
            this.chanceCharging.Location = new Point(0x9e, 0x38);
            this.chanceCharging.Name = "chanceCharging";
            this.chanceCharging.Size = new Size(0x2b, 20);
            this.chanceCharging.TabIndex = 10;
            this.chanceCharging.TextAlign = HorizontalAlignment.Right;
            int[] numArray6 = new int[4];
            numArray6[0] = 20;
            this.chanceCharging.Value = new decimal(numArray6);
            this.label18.AutoSize = true;
            this.label18.Location = new Point(0xcf, 20);
            this.label18.Name = "label18";
            this.label18.Size = new Size(30, 13);
            this.label18.TabIndex = 9;
            this.label18.Text = "/100";
            this.chanceDefault.Location = new Point(0x9e, 0x12);
            this.chanceDefault.Name = "chanceDefault";
            this.chanceDefault.Size = new Size(0x2b, 20);
            this.chanceDefault.TabIndex = 8;
            this.chanceDefault.TextAlign = HorizontalAlignment.Right;
            int[] numArray7 = new int[4];
            numArray7[0] = 20;
            this.chanceDefault.Value = new decimal(numArray7);
            this.label17.AutoSize = true;
            this.label17.Location = new Point(6, 0x53);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x5c, 13);
            this.label17.TabIndex = 7;
            this.label17.Text = "Duration (ms) from";
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x93, 0x53);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x10, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "to";
            this.maxStrafeMs.Location = new Point(0xa5, 0x51);
            int[] numArray8 = new int[4];
            numArray8[0] = 0x7d0;
            this.maxStrafeMs.Maximum = new decimal(numArray8);
            this.maxStrafeMs.Name = "maxStrafeMs";
            this.maxStrafeMs.Size = new Size(0x2b, 20);
            this.maxStrafeMs.TabIndex = 5;
            this.maxStrafeMs.TextAlign = HorizontalAlignment.Right;
            int[] numArray9 = new int[4];
            numArray9[0] = 800;
            this.maxStrafeMs.Value = new decimal(numArray9);
            this.minStrafeMs.Location = new Point(0x63, 0x51);
            int[] numArray10 = new int[4];
            numArray10[0] = 0x7d0;
            this.minStrafeMs.Maximum = new decimal(numArray10);
            this.minStrafeMs.Name = "minStrafeMs";
            this.minStrafeMs.Size = new Size(0x2b, 20);
            this.minStrafeMs.TabIndex = 4;
            this.minStrafeMs.TextAlign = HorizontalAlignment.Right;
            int[] numArray11 = new int[4];
            numArray11[0] = 600;
            this.minStrafeMs.Value = new decimal(numArray11);
            this.isStrafeOnCharge.AutoSize = true;
            this.isStrafeOnCharge.Location = new Point(0x21, 0x39);
            this.isStrafeOnCharge.Name = "isStrafeOnCharge";
            this.isStrafeOnCharge.Size = new Size(0x69, 0x11);
            this.isStrafeOnCharge.TabIndex = 3;
            this.isStrafeOnCharge.Text = "on Charging Skill";
            this.isStrafeOnCharge.UseVisualStyleBackColor = true;
            this.isStrafePlayer.AutoSize = true;
            this.isStrafePlayer.Checked = true;
            this.isStrafePlayer.CheckState = CheckState.Checked;
            this.isStrafePlayer.Location = new Point(0x21, 0x24);
            this.isStrafePlayer.Name = "isStrafePlayer";
            this.isStrafePlayer.Size = new Size(0x77, 0x11);
            this.isStrafePlayer.TabIndex = 2;
            this.isStrafePlayer.Text = "on Player Detection";
            this.isStrafePlayer.UseVisualStyleBackColor = true;
            this.isStrafeEnabled.AutoSize = true;
            this.isStrafeEnabled.Location = new Point(9, 0x13);
            this.isStrafeEnabled.Name = "isStrafeEnabled";
            this.isStrafeEnabled.Size = new Size(90, 0x11);
            this.isStrafeEnabled.TabIndex = 0;
            this.isStrafeEnabled.Text = "Enable Strafe";
            this.isStrafeEnabled.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.maxAggroDist);
            this.groupBox4.Controls.Add(this.maxPlayerDist);
            this.groupBox4.Controls.Add(this.isDetectAggro);
            this.groupBox4.Controls.Add(this.isDetectPlayer);
            this.groupBox4.Location = new Point(0x10c, 0x6b);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(200, 0x44);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Detect Option";
            this.maxAggroDist.Location = new Point(0x90, 0x24);
            this.maxAggroDist.Name = "maxAggroDist";
            this.maxAggroDist.Size = new Size(0x29, 20);
            this.maxAggroDist.TabIndex = 3;
            this.maxAggroDist.TextAlign = HorizontalAlignment.Right;
            int[] numArray12 = new int[4];
            numArray12[0] = 15;
            this.maxAggroDist.Value = new decimal(numArray12);
            this.maxPlayerDist.Location = new Point(0x90, 15);
            int[] numArray13 = new int[4];
            numArray13[0] = 0x2710;
            this.maxPlayerDist.Maximum = new decimal(numArray13);
            this.maxPlayerDist.Name = "maxPlayerDist";
            this.maxPlayerDist.Size = new Size(0x29, 20);
            this.maxPlayerDist.TabIndex = 2;
            this.maxPlayerDist.TextAlign = HorizontalAlignment.Right;
            int[] numArray14 = new int[4];
            numArray14[0] = 500;
            this.maxPlayerDist.Value = new decimal(numArray14);
            this.isDetectAggro.AutoSize = true;
            this.isDetectAggro.Checked = true;
            this.isDetectAggro.CheckState = CheckState.Checked;
            this.isDetectAggro.Location = new Point(7, 0x25);
            this.isDetectAggro.Name = "isDetectAggro";
            this.isDetectAggro.Size = new Size(0x83, 0x11);
            this.isDetectAggro.TabIndex = 1;
            this.isDetectAggro.Text = "Detect Aggro at range";
            this.isDetectAggro.UseVisualStyleBackColor = true;
            this.isDetectPlayer.AutoSize = true;
            this.isDetectPlayer.Checked = true;
            this.isDetectPlayer.CheckState = CheckState.Checked;
            this.isDetectPlayer.Location = new Point(6, 0x10);
            this.isDetectPlayer.Name = "isDetectPlayer";
            this.isDetectPlayer.Size = new Size(0x84, 0x11);
            this.isDetectPlayer.TabIndex = 0;
            this.isDetectPlayer.Text = "Detect Player at range";
            this.isDetectPlayer.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.isLogCast);
            this.groupBox2.Controls.Add(this.isLogChain);
            this.groupBox2.Controls.Add(this.isLogXP);
            this.groupBox2.Location = new Point(0x10c, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(200, 0x5f);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log Option";
            this.isLogCast.AutoSize = true;
            this.isLogCast.Checked = true;
            this.isLogCast.CheckState = CheckState.Checked;
            this.isLogCast.Location = new Point(6, 0x41);
            this.isLogCast.Name = "isLogCast";
            this.isLogCast.Size = new Size(0x68, 0x11);
            this.isLogCast.TabIndex = 5;
            this.isLogCast.Text = "Log Casting Skill";
            this.isLogCast.UseVisualStyleBackColor = true;
            this.isLogChain.AutoSize = true;
            this.isLogChain.Checked = true;
            this.isLogChain.CheckState = CheckState.Checked;
            this.isLogChain.Location = new Point(6, 0x2a);
            this.isLogChain.Name = "isLogChain";
            this.isLogChain.Size = new Size(0x60, 0x11);
            this.isLogChain.TabIndex = 4;
            this.isLogChain.Text = "Log Chain Skill";
            this.isLogChain.UseVisualStyleBackColor = true;
            this.isLogXP.AutoSize = true;
            this.isLogXP.Checked = true;
            this.isLogXP.CheckState = CheckState.Checked;
            this.isLogXP.Location = new Point(6, 0x13);
            this.isLogXP.Name = "isLogXP";
            this.isLogXP.Size = new Size(0x62, 0x11);
            this.isLogXP.TabIndex = 1;
            this.isLogXP.Text = "Log Loot Value";
            this.isLogXP.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.isBandage);
            this.groupBox1.Controls.Add(this.isMPItem);
            this.groupBox1.Controls.Add(this.isHPItem);
            this.groupBox1.Controls.Add(this.usePanacea);
            this.groupBox1.Controls.Add(this.useCampfire);
            this.groupBox1.Controls.Add(this.minStamina);
            this.groupBox1.Controls.Add(this.isStamina);
            this.groupBox1.Controls.Add(this.MinHpPerc);
            this.groupBox1.Controls.Add(this.MinMpPerc);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xf2, 0xc0);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rest Option";
            this.isBandage.AutoSize = true;
            this.isBandage.Checked = true;
            this.isBandage.CheckState = CheckState.Checked;
            this.isBandage.Location = new Point(9, 0xab);
            this.isBandage.Name = "isBandage";
            this.isBandage.Size = new Size(0x5b, 0x11);
            this.isBandage.TabIndex = 9;
            this.isBandage.Text = "Use Bandage";
            this.isBandage.UseVisualStyleBackColor = true;
            this.isMPItem.AutoSize = true;
            this.isMPItem.Checked = true;
            this.isMPItem.CheckState = CheckState.Checked;
            this.isMPItem.Location = new Point(9, 0x9a);
            this.isMPItem.Name = "isMPItem";
            this.isMPItem.Size = new Size(0x67, 0x11);
            this.isMPItem.TabIndex = 8;
            this.isMPItem.Text = "Use Mana Items";
            this.isMPItem.UseVisualStyleBackColor = true;
            this.isHPItem.AutoSize = true;
            this.isHPItem.Checked = true;
            this.isHPItem.CheckState = CheckState.Checked;
            this.isHPItem.Location = new Point(9, 0x89);
            this.isHPItem.Name = "isHPItem";
            this.isHPItem.Size = new Size(0x62, 0x11);
            this.isHPItem.TabIndex = 1;
            this.isHPItem.Text = "Use Heal Items";
            this.isHPItem.UseVisualStyleBackColor = true;
            this.usePanacea.AutoSize = true;
            this.usePanacea.Checked = true;
            this.usePanacea.CheckState = CheckState.Checked;
            this.usePanacea.Location = new Point(0x21, 0x60);
            this.usePanacea.Name = "usePanacea";
            this.usePanacea.Size = new Size(0x5b, 0x11);
            this.usePanacea.TabIndex = 7;
            this.usePanacea.Text = "Use Panacea";
            this.usePanacea.UseVisualStyleBackColor = true;
            this.useCampfire.AutoSize = true;
            this.useCampfire.Checked = true;
            this.useCampfire.CheckState = CheckState.Checked;
            this.useCampfire.Location = new Point(0x21, 0x72);
            this.useCampfire.Name = "useCampfire";
            this.useCampfire.Size = new Size(0x59, 0x11);
            this.useCampfire.TabIndex = 6;
            this.useCampfire.Text = "Use Campfire";
            this.useCampfire.UseVisualStyleBackColor = true;
            this.minStamina.Location = new Point(0xa8, 0x4d);
            int[] numArray15 = new int[4];
            numArray15[0] = 130;
            this.minStamina.Maximum = new decimal(numArray15);
            int[] numArray16 = new int[4];
            numArray16[0] = 20;
            this.minStamina.Minimum = new decimal(numArray16);
            this.minStamina.Name = "minStamina";
            this.minStamina.Size = new Size(50, 20);
            this.minStamina.TabIndex = 5;
            this.minStamina.TextAlign = HorizontalAlignment.Right;
            int[] numArray17 = new int[4];
            numArray17[0] = 20;
            this.minStamina.Value = new decimal(numArray17);
            this.isStamina.AutoSize = true;
            this.isStamina.Checked = true;
            this.isStamina.CheckState = CheckState.Checked;
            this.isStamina.Location = new Point(9, 0x4e);
            this.isStamina.Name = "isStamina";
            this.isStamina.Size = new Size(0x7f, 0x11);
            this.isStamina.TabIndex = 4;
            this.isStamina.Text = "Rest when Stamina <";
            this.isStamina.UseVisualStyleBackColor = true;
            this.MinHpPerc.Location = new Point(0xa8, 0x33);
            this.MinHpPerc.Name = "MinHpPerc";
            this.MinHpPerc.Size = new Size(50, 20);
            this.MinHpPerc.TabIndex = 3;
            this.MinHpPerc.TextAlign = HorizontalAlignment.Right;
            int[] numArray18 = new int[4];
            numArray18[0] = 40;
            this.MinHpPerc.Value = new decimal(numArray18);
            this.MinMpPerc.Location = new Point(0xa8, 0x1b);
            this.MinMpPerc.Name = "MinMpPerc";
            this.MinMpPerc.Size = new Size(50, 20);
            this.MinMpPerc.TabIndex = 2;
            this.MinMpPerc.TextAlign = HorizontalAlignment.Right;
            int[] numArray19 = new int[4];
            numArray19[0] = 40;
            this.MinMpPerc.Value = new decimal(numArray19);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(6, 0x35);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x76, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Use Heal when HP % <";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(6, 0x1d);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x7e, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Regen MP when MP % <";
            this.tabPage8.Controls.Add(this.groupBox3);
            this.tabPage8.Controls.Add(this.isMCLoot);
            this.tabPage8.Location = new Point(4, 0x16);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new Padding(3);
            this.tabPage8.Size = new Size(0x1ee, 0x13e);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "Loot Manager";
            this.tabPage8.UseVisualStyleBackColor = true;
            this.groupBox3.Controls.Add(this.lootEpic);
            this.groupBox3.Controls.Add(this.lootSuperior);
            this.groupBox3.Controls.Add(this.lootUncommon);
            this.groupBox3.Controls.Add(this.lootCommon);
            this.groupBox3.Controls.Add(this.isLootOnlyGold);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.maxLootDistance);
            this.groupBox3.Location = new Point(15, 0x23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xb1, 0xa6);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Loot Option";
            this.lootEpic.AutoSize = true;
            this.lootEpic.Checked = true;
            this.lootEpic.CheckState = CheckState.Checked;
            this.lootEpic.Location = new Point(9, 0x79);
            this.lootEpic.Name = "lootEpic";
            this.lootEpic.Size = new Size(0x6d, 0x11);
            this.lootEpic.TabIndex = 10;
            this.lootEpic.Text = "Loot Epic (yellow)";
            this.lootEpic.UseVisualStyleBackColor = true;
            this.lootSuperior.AutoSize = true;
            this.lootSuperior.Checked = true;
            this.lootSuperior.CheckState = CheckState.Checked;
            this.lootSuperior.Location = new Point(9, 0x62);
            this.lootSuperior.Name = "lootSuperior";
            this.lootSuperior.Size = new Size(0x66, 0x11);
            this.lootSuperior.TabIndex = 9;
            this.lootSuperior.Text = "Loot Rare (blue)";
            this.lootSuperior.UseVisualStyleBackColor = true;
            this.lootUncommon.AutoSize = true;
            this.lootUncommon.Checked = true;
            this.lootUncommon.CheckState = CheckState.Checked;
            this.lootUncommon.Location = new Point(9, 0x4b);
            this.lootUncommon.Name = "lootUncommon";
            this.lootUncommon.Size = new Size(140, 0x11);
            this.lootUncommon.TabIndex = 8;
            this.lootUncommon.Text = "Loot Uncommon (green)";
            this.lootUncommon.UseVisualStyleBackColor = true;
            this.lootCommon.AutoSize = true;
            this.lootCommon.Checked = true;
            this.lootCommon.CheckState = CheckState.Checked;
            this.lootCommon.Location = new Point(9, 0x34);
            this.lootCommon.Name = "lootCommon";
            this.lootCommon.Size = new Size(0x7d, 0x11);
            this.lootCommon.TabIndex = 7;
            this.lootCommon.Text = "Loot Common (white)";
            this.lootCommon.UseVisualStyleBackColor = true;
            this.isLootOnlyGold.AutoSize = true;
            this.isLootOnlyGold.Location = new Point(9, 0x20);
            this.isLootOnlyGold.Name = "isLootOnlyGold";
            this.isLootOnlyGold.Size = new Size(0x48, 0x11);
            this.isLootOnlyGold.TabIndex = 3;
            this.isLootOnlyGold.Text = "Gold Only";
            this.isLootOnlyGold.UseVisualStyleBackColor = true;
            this.isLootOnlyGold.CheckedChanged += new EventHandler(this.isLootOnlyGold_CheckedChanged);
            this.label13.AutoSize = true;
            this.label13.Location = new Point(6, 0x10);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x60, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Max Loot Distance";
            this.maxLootDistance.Location = new Point(0x7a, 14);
            int[] numArray20 = new int[4];
            numArray20[0] = 70;
            this.maxLootDistance.Maximum = new decimal(numArray20);
            int[] numArray21 = new int[4];
            numArray21[0] = 10;
            this.maxLootDistance.Minimum = new decimal(numArray21);
            this.maxLootDistance.Name = "maxLootDistance";
            this.maxLootDistance.Size = new Size(0x21, 20);
            this.maxLootDistance.TabIndex = 1;
            this.maxLootDistance.TextAlign = HorizontalAlignment.Right;
            int[] numArray22 = new int[4];
            numArray22[0] = 30;
            this.maxLootDistance.Value = new decimal(numArray22);
            this.isMCLoot.AutoSize = true;
            this.isMCLoot.Checked = true;
            this.isMCLoot.CheckState = CheckState.Checked;
            this.isMCLoot.Location = new Point(15, 12);
            this.isMCLoot.Name = "isMCLoot";
            this.isMCLoot.Size = new Size(0x92, 0x11);
            this.isMCLoot.TabIndex = 0;
            this.isMCLoot.Text = "Enalble MultiCombat Loot";
            this.isMCLoot.UseVisualStyleBackColor = true;
            this.btnOK.AccessibleName = "OK";
            this.btnOK.DialogResult = DialogResult.Yes;
            this.btnOK.Location = new Point(0x14d, 0x164);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4a, 0x21);
            this.btnOK.TabIndex = 0x12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnClear.AccessibleName = "Clear";
            this.btnClear.DialogResult = DialogResult.Yes;
            this.btnClear.Location = new Point(0xe0, 0x164);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x4a, 0x21);
            this.btnClear.TabIndex = 0x13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            base.AcceptButton = this.OK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ControlLightLight;
            base.CancelButton = this.Cancel;
            base.ClientSize = new Size(0x20b, 0x192);
            base.Controls.Add(this.btnClear);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.btnLoad);
            base.Controls.Add(this.Cancel);
            base.Controls.Add(this.OK);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "Settings";
            this.Text = "Settings";
            base.TopMost = true;
            base.Load += new EventHandler(this.Settings_Load);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.LRDistance.EndInit();
            this.CRDistance.EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.PullDistance.EndInit();
            this.Skills.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.waitMS.EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.chanceCharging.EndInit();
            this.chanceDefault.EndInit();
            this.maxStrafeMs.EndInit();
            this.minStrafeMs.EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.maxAggroDist.EndInit();
            this.maxPlayerDist.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.minStamina.EndInit();
            this.MinHpPerc.EndInit();
            this.MinMpPerc.EndInit();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.maxLootDistance.EndInit();
            base.ResumeLayout(false);
        }

        private void isLootOnlyGold_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isLootOnlyGold.Checked)
            {
                this.lootCommon.Checked = false;
                this.lootEpic.Checked = false;
                this.lootSuperior.Checked = false;
                this.lootUncommon.Checked = false;
            }
        }

        private void lernedSkillPull_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void lernedSkillPull_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.lernedSkill.Items.Count != 0)
            {
                this.lernedSkill.DoDragDrop(this.lernedSkill.SelectedItem.ToString(), DragDropEffects.Copy);
            }
        }

        private void lernedSkillPull_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadChainList()
        {
        }

        public void LoadGenericList(ListBox.ObjectCollection items, List<Skill> skills)
        {
            items.Clear();
            foreach (Skill skill in skills)
            {
                string item = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name);
                items.Add(item);
            }
        }

        public void LoadLearnedList()
        {
            this.lernedSkill.Items.Clear();
            foreach (KeyValuePair<uint, Structs.TERASkill> pair in MyTERA.Helpers.SkillsManager.SkillsManager.GetLearnedSkills())
            {
                if (pair.Value.Type != Enums.SpellTypes.Passive)
                {
                    string item = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(pair.Value.Name);
                    this.lernedSkill.Items.Add(item);
                }
            }
        }

        private void LoadSettingsData()
        {
            this.LoadLearnedList();
            this.LoadChainList();
            this.LoadGenericList(this.PullSkillList.Items, Globals.Settings.PullSkills);
            this.LoadGenericList(this.CombatSkillList.Items, Globals.Settings.CombatSkills);
            this.LoadGenericList(this.Combo.Items, Globals.Settings.ComboSkills);
            this.LoadGenericList(this.ManaMana.Items, Globals.Settings.MPRegenSkills);
            this.LoadGenericList(this.Double.Items, Globals.Settings.DoubleCastSkills);
            this.LoadGenericList(this.NoChain.Items, Globals.Settings.NoChainSkills);
            this.LoadGenericList(this.LongRange.Items, Globals.Settings.LongRangeSkills);
            this.LoadGenericList(this.HealList.Items, Globals.Settings.HealSkills);
            this.LoadGenericList(this.waitSkillList.Items, Globals.Settings.waitChainSkills);
            this.LoadGenericList(this.buffList.Items, Globals.Settings.Buffs);
            this.waitMSEnabler.Checked = Globals.Settings.isWait4Ms;
            this.waitMS.Value = Globals.Settings.wait4MS;
            this.LRDistance.Value = Globals.Settings.LongRange;
            this.CRDistance.Value = Globals.Settings.CloseRange;
            this.PullDistance.Value = Globals.Settings.PullRange;
            this.MinMpPerc.Value = Globals.Settings.MinMpPerc;
            this.MinHpPerc.Value = Globals.Settings.MinHpPerc;
            this.isLogXP.Checked = Globals.Settings.isLogXP;
            this.isLogChain.Checked = Globals.Settings.isLogChain;
            this.isLogCast.Checked = Globals.Settings.isLogCast;
            this.isMCLoot.Checked = Globals.Settings.Loot.isMCLoot;
            this.maxLootDistance.Value = Globals.Settings.Loot.maxLootDistance;
            this.isLootOnlyGold.Checked = Globals.Settings.Loot.isGoldOnly;
            this.isDetectAggro.Checked = Globals.Settings.isDetectAggro;
            this.isDetectPlayer.Checked = Globals.Settings.isDetectPlayer;
            this.maxAggroDist.Value = Globals.Settings.maxAggroDist;
            this.maxPlayerDist.Value = Globals.Settings.maxPlayerDist;
            this.isStrafeEnabled.Checked = Globals.Settings.isStrafe;
            this.isStamina.Checked = Globals.Settings.isStamina;
            this.useCampfire.Checked = Globals.Settings.useCampfire;
            this.usePanacea.Checked = Globals.Settings.usePanacea;
            this.minStamina.Value = Globals.Settings.minStamina;
            this.isBandage.Checked = Globals.Settings.isBandage;
            this.isHPItem.Checked = Globals.Settings.isHPItem;
            this.isMPItem.Checked = Globals.Settings.isMPItem;
            this.isStrafeOnCharge.Checked = Globals.Settings.isStrafeCharge;
            this.isStrafePlayer.Checked = Globals.Settings.isStrafePlayer;
            this.minStrafeMs.Value = Globals.Settings.minStrafeTime;
            this.maxStrafeMs.Value = Globals.Settings.maxStrafeTime;
            this.chanceCharging.Value = Globals.Settings.chanceCharging;
            this.chanceDefault.Value = Globals.Settings.chanceDefault;
            this.lootCommon.Checked = Globals.Settings.Loot.lootCommon;
            this.lootEpic.Checked = Globals.Settings.Loot.lootEpic;
            this.lootSuperior.Checked = Globals.Settings.Loot.lootSuperior;
            this.lootUncommon.Checked = Globals.Settings.Loot.lootRare;
        }

        private void LongRange_DoubleClick(object sender, EventArgs e)
        {
            this.LongRange.Items.Remove(this.LongRange.SelectedItem);
        }

        private void LongRange_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.LongRange.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void LongRange_DragOver(object sender, DragEventArgs e)
        {
            if (this.LongRange.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void ManaMana_DoubleClick(object sender, EventArgs e)
        {
            this.ManaMana.Items.Remove(this.ManaMana.SelectedItem);
        }

        private void ManaMana_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.ManaMana.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void ManaMana_DragOver(object sender, DragEventArgs e)
        {
            if (this.ManaMana.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void NoChain_DoubleClick(object sender, EventArgs e)
        {
            this.NoChain.Items.Remove(this.NoChain.SelectedItem);
        }

        private void NoChain_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.NoChain.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void NoChain_DragOver(object sender, DragEventArgs e)
        {
            if (this.NoChain.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.RefreshSettings();
            SaveFileDialog dialog = new SaveFileDialog {
                FileName = string.Concat(new object[] { "Settings_", Player.ClassName, "_Lv", Player.GetLevel() }),
                Title = "Save combat profile ...",
                Filter = "MultiCombat Profile (*.xml)|*.xml",
                DefaultExt = "xml",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer.Serialize(dialog.FileName, Globals.Settings);
                new IniFile(Application.StartupPath + @"\MC_Settings.ini").IniWriteValue("MultiCombat", "LastProfile", dialog.FileName);
            }
            base.Close();
        }

        private void PullSkillList_DoubleClick(object sender, EventArgs e)
        {
            this.PullSkillList.Items.Remove(this.PullSkillList.SelectedItem);
        }

        private void PullSkillList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.PullSkillList.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void PullSkillList_DragOver(object sender, DragEventArgs e)
        {
            if (this.PullSkillList.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void PullSkillList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void RefreshBuffList()
        {
            Globals.Settings.Buffs.Clear();
            foreach (object obj2 in this.buffList.Items)
            {
                try
                {
                    string itemText = this.buffList.GetItemText(obj2);
                    if (!Globals.Settings.Buffs.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.Buffs.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshCombatList()
        {
            Globals.Settings.CombatSkills.Clear();
            foreach (object obj2 in this.CombatSkillList.Items)
            {
                try
                {
                    string itemText = this.CombatSkillList.GetItemText(obj2);
                    if (!Globals.Settings.CombatSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.CombatSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshComboList()
        {
            Globals.Settings.ComboSkills.Clear();
            foreach (object obj2 in this.Combo.Items)
            {
                try
                {
                    string itemText = this.Combo.GetItemText(obj2);
                    if (!Globals.Settings.ComboSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.ComboSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshDoubleList()
        {
            Globals.Settings.DoubleCastSkills.Clear();
            foreach (object obj2 in this.Double.Items)
            {
                try
                {
                    string itemText = this.Double.GetItemText(obj2);
                    if (!Globals.Settings.DoubleCastSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.DoubleCastSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshHealList()
        {
            Globals.Settings.HealSkills.Clear();
            foreach (object obj2 in this.HealList.Items)
            {
                try
                {
                    string itemText = this.HealList.GetItemText(obj2);
                    if (!Globals.Settings.HealSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.HealSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshLongRangeList()
        {
            Globals.Settings.LongRangeSkills.Clear();
            foreach (object obj2 in this.LongRange.Items)
            {
                try
                {
                    string itemText = this.LongRange.GetItemText(obj2);
                    if (!Globals.Settings.LongRangeSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.LongRangeSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshManaManaList()
        {
            Globals.Settings.MPRegenSkills.Clear();
            foreach (object obj2 in this.ManaMana.Items)
            {
                try
                {
                    string itemText = this.ManaMana.GetItemText(obj2);
                    if (!Globals.Settings.MPRegenSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.MPRegenSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshNoChainList()
        {
            Globals.Settings.NoChainSkills.Clear();
            foreach (object obj2 in this.NoChain.Items)
            {
                try
                {
                    string itemText = this.NoChain.GetItemText(obj2);
                    if (!Globals.Settings.NoChainSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.NoChainSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshPullList()
        {
            Globals.Settings.PullSkills.Clear();
            foreach (object obj2 in this.PullSkillList.Items)
            {
                try
                {
                    string itemText = this.PullSkillList.GetItemText(obj2);
                    if (!Globals.Settings.PullSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.PullSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshSettings()
        {
            this.RefreshPullList();
            this.RefreshCombatList();
            this.RefreshComboList();
            this.RefreshManaManaList();
            this.RefreshNoChainList();
            this.RefreshDoubleList();
            this.RefreshLongRangeList();
            this.RefreshHealList();
            this.RefreshWaitList();
            this.RefreshBuffList();
            Globals.Settings.isLogCast = this.isLogCast.Checked;
            Globals.Settings.isLogChain = this.isLogChain.Checked;
            Globals.Settings.isLogXP = this.isLogXP.Checked;
            Globals.Settings.LongRange = (int) this.LRDistance.Value;
            Globals.Settings.CloseRange = (int) this.CRDistance.Value;
            Globals.Settings.PullRange = (int) this.PullDistance.Value;
            Globals.Settings.MinHpPerc = (int) this.MinHpPerc.Value;
            Globals.Settings.MinMpPerc = (int) this.MinMpPerc.Value;
            Globals.Settings.wait4MS = (int) this.waitMS.Value;
            Globals.Settings.isWait4Ms = this.waitMSEnabler.Checked;
            Globals.Settings.Loot.isMCLoot = this.isMCLoot.Checked;
            Globals.Settings.Loot.maxLootDistance = (int) this.maxLootDistance.Value;
            Globals.Settings.Loot.isGoldOnly = this.isLootOnlyGold.Checked;
            Globals.Settings.isDetectAggro = this.isDetectAggro.Checked;
            Globals.Settings.maxAggroDist = (int) this.maxAggroDist.Value;
            Globals.Settings.isDetectPlayer = this.isDetectPlayer.Checked;
            Globals.Settings.maxPlayerDist = (int) this.maxPlayerDist.Value;
            Globals.Settings.isStrafe = this.isStrafeEnabled.Checked;
            Globals.Settings.isStamina = this.isStamina.Checked;
            Globals.Settings.useCampfire = this.useCampfire.Checked;
            Globals.Settings.usePanacea = this.usePanacea.Checked;
            Globals.Settings.minStamina = (int) this.minStamina.Value;
            Globals.Settings.isBandage = this.isBandage.Checked;
            Globals.Settings.isHPItem = this.isHPItem.Checked;
            Globals.Settings.isMPItem = this.isMPItem.Checked;
            Globals.Settings.minStrafeTime = (int) this.minStrafeMs.Value;
            Globals.Settings.maxStrafeTime = (int) this.maxStrafeMs.Value;
            Globals.Settings.chanceCharging = (int) this.chanceCharging.Value;
            Globals.Settings.chanceDefault = (int) this.chanceDefault.Value;
            Globals.Settings.isStrafeCharge = this.isStrafeOnCharge.Checked;
            Globals.Settings.isStrafePlayer = this.isStrafePlayer.Checked;
            Globals.Settings.Loot.lootCommon = this.lootCommon.Checked;
            Globals.Settings.Loot.lootEpic = this.lootEpic.Checked;
            Globals.Settings.Loot.lootRare = this.lootUncommon.Checked;
            Globals.Settings.Loot.lootSuperior = this.lootSuperior.Checked;
        }

        private void RefreshWaitList()
        {
            Globals.Settings.waitChainSkills.Clear();
            foreach (object obj2 in this.waitSkillList.Items)
            {
                try
                {
                    string itemText = this.waitSkillList.GetItemText(obj2);
                    if (!Globals.Settings.waitChainSkills.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        Globals.Settings.waitChainSkills.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.Text = string.Concat(new object[] { "Settings for ", Player.ClassName, " (", Player.GetName(), " Level ", Player.GetLevel(), ")" });
            this.LoadSettingsData();
        }

        private void waitSkillList_DoubleClick(object sender, EventArgs e)
        {
            this.waitSkillList.Items.Remove(this.waitSkillList.SelectedItem);
        }

        private void waitSkillList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.waitSkillList.Items.Add(e.Data.GetData(DataFormats.StringFormat));
            }
        }

        private void waitSkillList_DragOver(object sender, DragEventArgs e)
        {
            if (this.waitSkillList.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }
    }
}

