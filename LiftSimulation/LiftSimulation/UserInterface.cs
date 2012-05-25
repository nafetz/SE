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

        Label[] floor_numbers;
        Label[] current_position;
        GroupBox[] floors;
        CheckedListBox[] required;
        public UserInterface()
        {
            InitializeComponent();
            floors = new GroupBox[Defaults.Floors];
            floor_numbers = new Label[Defaults.Floors];
            current_position = new Label[Defaults.Floors];
            required = new CheckedListBox[Defaults.Floors];

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
                    default: floor_numbers[i].Text = i + ". OG"; break;
                }
                floor_numbers[i].Width = 70;
                floor_numbers[i].Location = new Point(10,10);
                floor_numbers[i].Name = "label_floor" + i;
                floors[i].Controls.Add(floor_numbers[i]);
                floor_numbers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

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

    }
}
