using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiftSimulation
{

      
    public partial class UserInterface : Form
    {

        #region member

        private Label[] floor_numbers; //Anzeige der Etatennummern
        private Label[] current_position; //Anzeige der aktuellen Etage (gleich in allen Labels)
        private GroupBox[] floors;    //GroupBoxen für die Etagen
        private CheckedListBox[] required; //Fahrwünsche in den jeweiligen Etagen
        private PictureBox[] doorstates;
        private Image door1;
        private Image door2;

        #endregion


        public UserInterface()
        {
            InitializeComponent(); //Intialisierung der statisch erzeugten Formularelemente

            floors = new GroupBox[Defaults.Floors];
            floor_numbers = new Label[Defaults.Floors];
            current_position = new Label[Defaults.Floors];
            required = new CheckedListBox[Defaults.Floors];
            doorstates = new PictureBox[Defaults.Floors];

            //Ich bekommen einfach den relativen Pfad nicht rein....versuch du mal dein Glück :)
            door1 = Image.FromFile("C:/4.Semester/SE2/SE/LiftSimulation/LiftSimulation/Pictures/zu.png");
            door2 = Image.FromFile("C:/4.Semester/SE2/SE/LiftSimulation/LiftSimulation/Pictures/auf.png");


            for (int i = Defaults.Floors -1 ; i >= 0; i--)
            {
                floors[i] = new GroupBox();
                floors[i].Text = "";
                floors[i].Location = new Point(30, Defaults.Floors*100 - 110*i );
                floors[i].Height = 110;
                floors[i].Width = 350;
                floors[i].Name = "groupBox_floor" + i;
                groupBox1.Controls.Add(floors[i]);

                
                floor_numbers[i] = new Label();
                switch (i){               
                    case 0: floor_numbers[i].Text = "1. UG"; break;
                    case 1: floor_numbers[i].Text = "EG"; break;
                    default: floor_numbers[i].Text = (i -1) + ". OG"; break;
                }
                floor_numbers[i].Width = 70;
                floor_numbers[i].Location = new Point(10,10);
                floor_numbers[i].Name = "label_floor" + i;
                floors[i].Controls.Add(floor_numbers[i]);
                floor_numbers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                doorstates[i] = new PictureBox();
                doorstates[i].Width = 30;
                doorstates[i].Height = 30;
                doorstates[i].Location = new Point(100, 50);
                doorstates[i].Name = "pictureBox_doorstate" + i;
                doorstates[i].Image = door1;
                floors[i].Controls.Add(doorstates[i]);
                
                current_position[i] = new Label();
                current_position[i].Text = "#Position";
                current_position[i].Name = "label_position" + i;
                current_position[i].Width = 60;
                current_position[i].Location = new Point(100, 12);
                floors[i].Controls.Add(current_position[i]);
                

                required[i] = new CheckedListBox();
                required[i].Location = new Point(210, 20);
                required[i].Height = 50;
                required[i].Name = "checkedListBox_floor" + i;
                required[i].BackColor = System.Drawing.SystemColors.Control;
                if(i!=Defaults.Floors - 1 ) required[i].Items.Add("Aufwärts");
                if(i!=0)               required[i].Items.Add("Abwärts");
                required[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
                required[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                floors[i].Controls.Add(required[i]); 

                
                
            }

            
            
        }

        public List<bool> get_upwardRequired(){ //gibt aktualisierte Liste mit Aufwärtswünschen zurück

            List<bool> _upwards = new List<bool>();
            for(int i=0; i<Defaults.Floors;i++){
                if (i == Defaults.Floors - 1)
                {    _upwards.Add(false);
                }
                else{
                    if (required[i].GetItemCheckState(0) == CheckState.Checked) _upwards.Add(true);
                    else _upwards.Add(false);
                }
            }
            return _upwards;
        }

        public List<bool> get_downwardRequired()
        { //gibt aktualisierte Liste mit Aufwärtswünschen zurück

            List<bool> _downwards = new List<bool>();
            for (int i = 0; i < Defaults.Floors; i++)
            {
                if (i == 0)
                {
                    _downwards.Add(false);
                }
                else
                {
                    if (required[i].GetItemChecked(1)) _downwards.Add(true);
                    else _downwards.Add(false);
                }
            }
            return _downwards;
        }

        public List<bool> get_internRequired()
        {
            List<bool> _interns = new List<bool>();
            for (int i = checkedListBox1.Items.Count; i > 0; i--)
            {
                if(checkedListBox1.GetItemChecked(i)) _interns.Add(true);
                else _interns.Add(false);
            }
            return _interns;
        }


        public void set_pessenger(int Count)
        {
            label5.Text = Count.ToString();
            if (Count > Defaults.MaximumPassengers)
            {
                label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label5.ForeColor = Color.Red;
            }
            else
            {
                label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label5.ForeColor = Color.Black;
            }
            

        }

        public void set_current_position(int pos)
        {
            String position_value;
            if (pos > Defaults.Floors || pos < 0) return;
            switch (pos)
            {
                case 0: position_value = "1. UG"; break;
                case 1: position_value = "EG"; break;
                default: position_value = pos + ". OG"; break;
            }

            foreach (Label poslabel in current_position)
            {
                poslabel.Text = position_value; //äußere Label
            }

            label1.Text = position_value; //Label im inneren
        }

        public void floor_reached(int floor) //wenn eine Etage erreich wurde, müssen die Haltewünsche von der GUI verschwinden (innen + außen)
        {
            if (floor > Defaults.Floors || floor < 0) return;
            int itemnumber = Defaults.Floors - floor; //Reihenfolge auf der GUI entgegengesetzt zur Liste
            checkedListBox1.SetItemChecked(floor, false);
            if (true)  // HILFE, WIE KANN ICH DEN STATUS DES ENUMS RICHTUNG ABFRAGEN???!?!?!?!?!??!?!?!?!?!?!?!?!?!?!!?!?!
            {
                if (floor != Defaults.Floors) required[floor].SetItemChecked(0, false);
            }
            else
            {
                if(floor!=0)
                required[floor].SetItemChecked(1, false);
            }

        }

        public void set_direction(int direction){
            //Ändert die Bilder-Anzeige der Innen-Richtungs-Ansicht
            //Wieder das Problem, wie Frage ich den Status des ENUMS ab?!
        }

        public void open_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            if (doorstates[floor].Image == door1) doorstates[floor].Image = door2;
        }

        public void close_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            if (doorstates[floor].Image == door2) doorstates[floor].Image = door1;

        }




 

    }
}
