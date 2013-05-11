namespace MultiCombat.Forms
{
    using MultiCombat;
    using MultiCombat.Classes;
    using MyTERA.Helpers.SkillsManager;
    using MyTERA.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ComboManager : Form
    {
        private Button AddCombo;
        private Button btnDown;
        private Button btnOK;
        private Button btnUp;
        private Button Cancel;
        private TextBox ComboName;
        private ListBox ComboSkill;
        private ListBox CombosName;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label5;
        private ListBox LearnedSkill;
        private List<Combos> rotations;

        public ComboManager()
        {
            this.rotations = new List<Combos>();
            this.InitializeComponent();
        }

        public ComboManager(MultiCombat.Classes.Settings s)
        {
            this.rotations = new List<Combos>();
            this.InitializeComponent();
        }

        private void AddRotation(Combos c)
        {
            this.RemoveRotation(c);
            this.rotations.Add(c);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ComboManager_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void ComboName_Enter(object sender, EventArgs e)
        {
            Combos c = this.ReturnRotationByName(this.ComboName.Text);
            this.ComboSkill.Items.Clear();
            if (c == null)
            {
                c.ComboName = this.ComboName.Text;
                c.ComboSkills.Clear();
                this.CombosName.Items.Add(this.ComboName.Text);
            }
            this.LoadGenericList(this.ComboSkill.Items, c.ComboSkills);
            this.AddRotation(c);
        }

        private void ComboSkill_DoubleClick(object sender, EventArgs e)
        {
            this.ComboSkill.Items.Remove(this.ComboSkill.SelectedItem);
            this.SaveRotation();
        }

        private void ComboSkill_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                this.ComboSkill.Items.Add(e.Data.GetData(DataFormats.StringFormat));
                this.SaveRotation();
            }
        }

        private void ComboSkill_DragOver(object sender, DragEventArgs e)
        {
            if (this.ComboSkill.Items.Contains(e.Data.GetData(DataFormats.StringFormat)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void CombosName_Click(object sender, EventArgs e)
        {
            foreach (Combos combos in Globals.Settings.NewComboSkills)
            {
                if (this.CombosName.GetItemText(this.CombosName.SelectedItem) == combos.ComboName)
                {
                    this.SaveRotation();
                    this.ComboSkill.Items.Clear();
                    this.ComboName.Text = combos.ComboName;
                    this.LoadGenericList(this.ComboSkill.Items, combos.ComboSkills);
                    break;
                }
            }
        }

        private void CombosName_DoubleClick(object sender, EventArgs e)
        {
            this.RemoveRotation(this.ReturnRotationByName(this.CombosName.GetItemText(this.CombosName.SelectedItem)));
            this.ComboSkill.Items.Remove(this.ComboSkill.SelectedItem);
        }

        public Skill ConvertToSkill(object item)
        {
            Structs.TERASkill tERASkillByDataName = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillByDataName(this.LearnedSkill.GetItemText(item));
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

        private void InitializeComponent()
        {
            this.LearnedSkill = new ListBox();
            this.ComboSkill = new ListBox();
            this.AddCombo = new Button();
            this.Cancel = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.btnUp = new Button();
            this.btnDown = new Button();
            this.CombosName = new ListBox();
            this.ComboName = new TextBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.LearnedSkill.FormattingEnabled = true;
            this.LearnedSkill.Location = new Point(13, 0x47);
            this.LearnedSkill.Name = "LearnedSkill";
            this.LearnedSkill.Size = new Size(120, 160);
            this.LearnedSkill.TabIndex = 1;
            this.LearnedSkill.MouseDown += new MouseEventHandler(this.LearnedSkill_MouseDown);
            this.ComboSkill.FormattingEnabled = true;
            this.ComboSkill.Location = new Point(0xa5, 0xa2);
            this.ComboSkill.Name = "ComboSkill";
            this.ComboSkill.Size = new Size(120, 0x45);
            this.ComboSkill.TabIndex = 2;
            this.ComboSkill.DragDrop += new DragEventHandler(this.ComboSkill_DragDrop);
            this.ComboSkill.DragOver += new DragEventHandler(this.ComboSkill_DragOver);
            this.ComboSkill.DoubleClick += new EventHandler(this.ComboSkill_DoubleClick);
            this.AddCombo.Location = new Point(13, 0xf6);
            this.AddCombo.Name = "AddCombo";
            this.AddCombo.Size = new Size(0x36, 0x17);
            this.AddCombo.TabIndex = 3;
            this.AddCombo.Text = "New";
            this.AddCombo.UseVisualStyleBackColor = true;
            this.Cancel.Location = new Point(0xe8, 0xf6);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new Size(0x36, 0x17);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new EventHandler(this.Cancel_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xa3, 0x92);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1f, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Skills";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Learned Skills";
            this.btnUp.Location = new Point(0x124, 0x47);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new Size(0x10, 0x17);
            this.btnUp.TabIndex = 8;
            this.btnUp.Text = "^";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnDown.Location = new Point(0x124, 0x75);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new Size(0x10, 0x17);
            this.btnDown.TabIndex = 9;
            this.btnDown.Text = "v";
            this.btnDown.UseVisualStyleBackColor = true;
            this.CombosName.FormattingEnabled = true;
            this.CombosName.Location = new Point(0xa6, 0x47);
            this.CombosName.Name = "CombosName";
            this.CombosName.Size = new Size(120, 0x45);
            this.CombosName.TabIndex = 10;
            this.CombosName.Click += new EventHandler(this.CombosName_Click);
            this.CombosName.DoubleClick += new EventHandler(this.CombosName_DoubleClick);
            this.ComboName.Location = new Point(15, 0x20);
            this.ComboName.Name = "ComboName";
            this.ComboName.Size = new Size(0x76, 20);
            this.ComboName.TabIndex = 12;
            this.ComboName.Enter += new EventHandler(this.ComboName_Enter);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x23, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Name";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xa5, 0x37);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x26, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Priority";
            this.btnOK.Location = new Point(0xa5, 0xf6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x36, 0x17);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ControlLightLight;
            base.CancelButton = this.Cancel;
            base.ClientSize = new Size(0x13c, 0x11d);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.ComboName);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.CombosName);
            base.Controls.Add(this.btnDown);
            base.Controls.Add(this.btnUp);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Cancel);
            base.Controls.Add(this.AddCombo);
            base.Controls.Add(this.ComboSkill);
            base.Controls.Add(this.LearnedSkill);
            base.FormBorderStyle = FormBorderStyle.Fixed3D;
            base.Name = "ComboManager";
            this.Text = "Rotation Manager";
            base.TopMost = true;
            base.Load += new EventHandler(this.ComboManager_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LearnedSkill_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.LearnedSkill.Items.Count != 0)
            {
                this.LearnedSkill.DoDragDrop(this.LearnedSkill.SelectedItem.ToString(), DragDropEffects.Copy);
            }
        }

        private void LoadCombos()
        {
            this.rotations.Clear();
            if (Globals.Settings.NewComboSkills.Count == 0)
            {
                Combos item = new Combos {
                    ComboName = "Old Rotation",
                    ComboSkills = Globals.Settings.ComboSkills
                };
                Globals.Settings.NewComboSkills.Add(item);
            }
            foreach (Combos combos2 in Globals.Settings.NewComboSkills)
            {
                this.CombosName.Items.Add(combos2.ComboName);
                bool flag = true;
                if (flag)
                {
                    flag = false;
                    this.LoadGenericList(this.ComboSkill.Items, combos2.ComboSkills);
                }
                this.rotations.Add(combos2);
            }
        }

        private void LoadData()
        {
            this.LoadLearnedList();
            this.LoadCombos();
        }

        private void LoadGenericList(ListBox.ObjectCollection items, List<Skill> skills)
        {
            items.Clear();
            foreach (Skill skill in skills)
            {
                string item = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name);
                items.Add(item);
            }
        }

        private void LoadLearnedList()
        {
            this.LearnedSkill.Items.Clear();
            foreach (KeyValuePair<uint, Structs.TERASkill> pair in MyTERA.Helpers.SkillsManager.SkillsManager.GetLearnedSkills())
            {
                if (pair.Value.Type != Enums.SpellTypes.Passive)
                {
                    string item = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(pair.Value.Name);
                    this.LearnedSkill.Items.Add(item);
                }
            }
        }

        private void RefreshRotation()
        {
            this.RefreshRotationList();
        }

        private void RefreshRotationList()
        {
            Globals.Settings.NewComboSkills.Clear();
            foreach (Combos combos in this.rotations)
            {
                Globals.Settings.NewComboSkills.Add(combos);
            }
        }

        private void RemoveRotation(Combos c)
        {
            Combos item = this.ReturnRotationByName(c.ComboName);
            if (item != null)
            {
                this.rotations.Remove(item);
            }
        }

        private Combos ReturnRotationByName(string name)
        {
            return this.rotations.Find(cb => cb.ComboName == name);
        }

        private void SaveRotation()
        {
            List<Skill> list = new List<Skill>();
            Combos c = new Combos {
                ComboName = this.ComboName.Text
            };
            foreach (object obj2 in this.ComboSkill.Items)
            {
                try
                {
                    string itemText = this.ComboSkill.GetItemText(obj2);
                    if (!list.Contains(this.ConvertToSkill(obj2)) && (itemText.Length > 0))
                    {
                        list.Add(this.ConvertToSkill(obj2));
                    }
                }
                catch
                {
                }
            }
            this.AddRotation(c);
        }
    }
}

