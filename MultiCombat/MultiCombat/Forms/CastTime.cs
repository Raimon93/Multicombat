namespace MultiCombat.Forms
{
    using MultiCombat;
    using MultiCombat.Classes;
    using MyTERA.Helpers.SkillsManager;
    using MyTERA.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class CastTime : Form
    {
        private Button btnOK;
        private IContainer components;
        private DataGridView dataCastTime;
        private DataGridViewTextBoxColumn skillCastTime;
        public DataTable skillList = new DataTable();
        private DataGridViewTextBoxColumn skillName;

        public CastTime()
        {
            this.InitializeComponent();
            this.LoadData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataCastTime.Rows.Count; i++)
            {
                int num2;
                if (!int.TryParse(this.dataCastTime.Rows[i].Cells["skillCastTime"].Value.ToString().Trim(), out num2))
                {
                    num2 = 0;
                }
                string name = this.dataCastTime.Rows[i].Cells["skillName"].Value.ToString().Trim();
                this.SetCastTime(name, num2);
            }
            base.Close();
        }

        private void CastTime_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        public Skill ConvertToSkill(string name)
        {
            Structs.TERASkill tERASkillByDataName = MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillByDataName(name);
            return new Skill { SkillId = tERASkillByDataName.Id, Name = tERASkillByDataName.Name, CastTimeSeconds = 0, DoubleAction = false, MaxRange = 0, MinRange = 0 };
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
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
            this.dataCastTime = new DataGridView();
            this.btnOK = new Button();
            this.skillName = new DataGridViewTextBoxColumn();
            this.skillCastTime = new DataGridViewTextBoxColumn();
            ((ISupportInitialize) this.dataCastTime).BeginInit();
            base.SuspendLayout();
            this.dataCastTime.AllowUserToAddRows = false;
            this.dataCastTime.AllowUserToDeleteRows = false;
            this.dataCastTime.AllowUserToOrderColumns = true;
            this.dataCastTime.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataCastTime.Columns.AddRange(new DataGridViewColumn[] { this.skillName, this.skillCastTime });
            this.dataCastTime.Location = new Point(0, 0);
            this.dataCastTime.Name = "dataCastTime";
            this.dataCastTime.RowHeadersVisible = false;
            this.dataCastTime.ScrollBars = ScrollBars.Vertical;
            this.dataCastTime.Size = new Size(0xbb, 230);
            this.dataCastTime.TabIndex = 0;
            this.dataCastTime.CellContentClick += new DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.btnOK.Location = new Point(0x43, 0xec);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x36, 0x17);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.skillName.HeaderText = "Skill";
            this.skillName.Name = "skillName";
            this.skillName.ReadOnly = true;
            this.skillCastTime.HeaderText = "ms";
            this.skillCastTime.Name = "skillCastTime";
            this.skillCastTime.Width = 60;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ControlLightLight;
            base.ClientSize = new Size(0xbd, 0x106);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.dataCastTime);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "CastTime";
            this.Text = "CastTime";
            base.TopMost = true;
            base.Load += new EventHandler(this.CastTime_Load);
            ((ISupportInitialize) this.dataCastTime).EndInit();
            base.ResumeLayout(false);
        }

        private bool isThereSkill(string name)
        {
            foreach (Skill skill in Globals.Settings.castTimeSkills)
            {
                if (MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name).Equals(MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(name)))
                {
                    Globals.Settings.castTimeSkills.Add(this.UpdateSkill(skill, skill.CastTimeSeconds));
                    return true;
                }
            }
            return false;
        }

        private void LoadData()
        {
            this.LoadLearnedSkills();
        }

        private void LoadLearnedSkills()
        {
            this.dataCastTime.Rows.Clear();
            foreach (KeyValuePair<uint, Structs.TERASkill> pair in MyTERA.Helpers.SkillsManager.SkillsManager.GetLearnedSkills())
            {
                if (pair.Value.Type != Enums.SpellTypes.Passive)
                {
                    string name = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(pair.Value.Name);
                    if (!this.isThereSkill(pair.Value.Name))
                    {
                        Globals.Settings.castTimeSkills.Add(this.ConvertToSkill(name));
                    }
                }
            }
            foreach (Skill skill in Globals.Settings.castTimeSkills)
            {
                this.dataCastTime.Rows.Add(new object[] { MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name), skill.CastTimeSeconds });
            }
        }

        private void SetCastTime(string name, int castTime)
        {
            foreach (Skill skill in Globals.Settings.castTimeSkills)
            {
                if (MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name).Equals(name))
                {
                    Globals.Settings.castTimeSkills.Add(this.UpdateSkill(skill, castTime));
                    break;
                }
            }
        }

        private Skill UpdateSkill(Skill skill, int castTime)
        {
            Globals.Settings.castTimeSkills.Remove(skill);
            string name = MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(MyTERA.Helpers.SkillsManager.SkillsManager.GetTERASkillByDataName(MyTERA.Helpers.SkillsManager.SkillsManager.SkillNameTruncateRank(skill.Name)).Name);
            Skill skill2 = this.ConvertToSkill(name);
            skill2.CastTimeSeconds = castTime;
            return skill2;
        }
    }
}

