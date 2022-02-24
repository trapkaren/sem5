using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spasskaya_tower
{
    public partial class Form1 : Form
    {
        FacadeViewer facade;
        List<Primitive> primitives;
        Primitive primitive;
        

        Vec3d V = new Vec3d(0, 0, 0);
        public Form1()
        {
            InitializeComponent();

            this.facade = new FacadeViewer(canvas.Width, canvas.Height);

            

            ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");
            ToolStripMenuItem loadItem = new ToolStripMenuItem("Загрузить");
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Сохранить");

            
            fileItem.DropDownItems.Add(loadItem);
            fileItem.DropDownItems.Add(saveItem);

            openFileDialog1.Filter = "Text files(*.json)|*.json";
            saveFileDialog1.Filter = "Text files(*.json)|*.json";

            saveItem.Click += saveItem_Click;
            loadItem.Click += loadItem_Click;
            menuStrip1.Items.Add(fileItem);

            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");

            aboutItem.Click += aboutItem_Click;

            menuStrip1.Items.Add(aboutItem);

            render();
           
          
        }

        void render()
        {
            Command renderCommand = new RenderCommand(ref canvas, checkBox1.Checked);
            
            facade.executeCommand(renderCommand);
        }

       
        void saveItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog1.FileName;
            Command saveCommand = new SaveCommand(filename);
            facade.executeCommand(saveCommand);
            MessageBox.Show("Сцена сохранилась успешно");    
        }

        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Создание собственных сооружений, состоящих из сфер, цилиндров, конусов, параллелепипедов, треугольных и четырехугольных пирамид.");
        }


        void loadItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = openFileDialog1.FileName;

            try
            {
                Command loadCommand = new LoadCommand(filename);
                facade.executeCommand(loadCommand);
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка загрузки сцены! Некорректный файл!");
                return;
            }

            render();
            UpdateLightsName();
            comboBoxLights.SelectedIndex = 1;
            GetCameraCommand getCameraCommand = new GetCameraCommand();
            facade.executeCommand(getCameraCommand);
            Camera tmp = getCameraCommand.getResult();
            CameraPosX.Text = tmp.position.x.ToString();
            CameraPosY.Text = tmp.position.y.ToString();
            CameraPosZ.Text = tmp.position.z.ToString();
            CameraRotOX.Text = tmp.angle.x.ToString();
            CameraRotOY.Text = tmp.angle.y.ToString();
            CameraRotOZ.Text = tmp.angle.z.ToString();

            MessageBox.Show("Сцена загрузилась успешно");

        }


     

        private void VisibleCylinderParametrs(bool isVisible)
        {
            labelCylinderC.Visible = isVisible;
            labelCylinderR.Visible = isVisible;
            labelCylinderV.Visible = isVisible;
            labelCylinderCX.Visible = isVisible;
            labelCylinderCY.Visible = isVisible;
            labelCylinderCZ.Visible = isVisible;
            labelCylinderVX.Visible = isVisible;
            labelCylinderVY.Visible = isVisible;
            labelCylinderVZ.Visible = isVisible;
            labelCylinderH.Visible = isVisible;

            CylinderCX.Visible = isVisible;
            CylinderCY.Visible = isVisible;
            CylinderCZ.Visible = isVisible;
            CylinderVX.Visible = isVisible;
            CylinderVY.Visible = isVisible;
            CylinderVZ.Visible = isVisible;
            CylinderR.Visible = isVisible;
            CylinderH.Visible = isVisible;
        }

        private void VisibleConeParametrs(bool isVisible)
        {
            labelConeC.Visible = isVisible;
            labelConeK.Visible = isVisible;
            labelConeFrom.Visible = isVisible;
            labelConeTo.Visible = isVisible;
            labelConeGrad.Visible = isVisible;
            labelConeV.Visible = isVisible;
            labelConeCX.Visible = isVisible;
            labelConeCY.Visible = isVisible;
            labelConeCZ.Visible = isVisible;
            labelConeVX.Visible = isVisible;
            labelConeVY.Visible = isVisible;
            labelConeVZ.Visible = isVisible;
            labelConeH.Visible = isVisible;


            ConeCX.Visible = isVisible;
            ConeCY.Visible = isVisible;
            ConeCZ.Visible = isVisible;
            ConeVX.Visible = isVisible;
            ConeVY.Visible = isVisible;
            ConeVZ.Visible = isVisible;
            ConeK.Visible = isVisible;
            ConeMaxm.Visible = isVisible;
            ConeMinm.Visible = isVisible;
        }

        private void ColorBox_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
                // установка цвета формы
            ColorBox.BackColor = colorDialog1.Color;
        }

        private void VisibleSphereParametrs(bool isVisible)
        {
            labelSphereC.Visible = isVisible;
            labelSphereR.Visible = isVisible;
            labelSphereCX.Visible = isVisible;
            labelSphereCY.Visible = isVisible;
            labelSphereCZ.Visible = isVisible;

            SphereCX.Visible = isVisible;
            SphereCY.Visible = isVisible;
            SphereCZ.Visible = isVisible;
            SphereR.Visible = isVisible;
        }

        
        private void VisibleParallelipipedParametrs(bool isVisible)
        {
            labelParalC1.Visible = isVisible;
            labelParalE1.Visible = isVisible;
            labelParalC2.Visible = isVisible;
            labelParalE2.Visible = isVisible;
            labelParalCX.Visible = isVisible;
            labelParalCY.Visible = isVisible;
            labelParalCZ.Visible = isVisible;
            labelParalEX.Visible = isVisible;
            labelParalEY.Visible = isVisible;
            labelParalEZ.Visible = isVisible;

            ParalCX.Visible = isVisible;
            ParalCY.Visible = isVisible;
            ParalCZ.Visible = isVisible;
            ParalEX.Visible = isVisible;
            ParalEY.Visible = isVisible;
            ParalEZ.Visible = isVisible;
        }

        private void VisibleChoiceParallelepipedParametrs(bool isVisible)
        {
            labelChoiceParalC1.Visible = isVisible;
            labelChoiceParalE1.Visible = isVisible;
            labelChoiceParalC2.Visible = isVisible;
            labelChoiceParalE2.Visible = isVisible;
            labelChoiceParalCX.Visible = isVisible;
            labelChoiceParalCY.Visible = isVisible;
            labelChoiceParalCZ.Visible = isVisible;
            labelChoiceParalEX.Visible = isVisible;
            labelChoiceParalEY.Visible = isVisible;
            labelChoiceParalEZ.Visible = isVisible;

            ChoiceParalCX.Visible = isVisible;
            ChoiceParalCY.Visible = isVisible;
            ChoiceParalCZ.Visible = isVisible;
            ChoiceParalEX.Visible = isVisible;
            ChoiceParalEY.Visible = isVisible;
            ChoiceParalEZ.Visible = isVisible;
        }
        private void VisibleQuadPyramidParametrs(bool isVisible)
        {
            labelQuadPyramidP.Visible = isVisible;
            labelQuadPyramidA.Visible = isVisible;
            labelQuadPyramidB.Visible = isVisible;
            labelQuadPyramidC.Visible = isVisible;
            labelQuadPyramidD.Visible = isVisible;

            labelQuadPyramidPX.Visible = isVisible;
            labelQuadPyramidAX.Visible = isVisible;
            labelQuadPyramidBX.Visible = isVisible;
            labelQuadPyramidCX.Visible = isVisible;
            labelQuadPyramidDX.Visible = isVisible;

            labelQuadPyramidPY.Visible = isVisible;
            labelQuadPyramidAY.Visible = isVisible;
            labelQuadPyramidBY.Visible = isVisible;
            labelQuadPyramidCY.Visible = isVisible;
            labelQuadPyramidDY.Visible = isVisible;

            labelQuadPyramidPZ.Visible = isVisible;
            labelQuadPyramidAZ.Visible = isVisible;
            labelQuadPyramidBZ.Visible = isVisible;
            labelQuadPyramidCZ.Visible = isVisible;
            labelQuadPyramidDZ.Visible = isVisible;

            QuadPyramidPX.Visible = isVisible;
            QuadPyramidAX.Visible = isVisible;
            QuadPyramidBX.Visible = isVisible;
            QuadPyramidCX.Visible = isVisible;
            QuadPyramidDX.Visible = isVisible;

            QuadPyramidPY.Visible = isVisible;
            QuadPyramidAY.Visible = isVisible;
            QuadPyramidBY.Visible = isVisible;
            QuadPyramidCY.Visible = isVisible;
            QuadPyramidDY.Visible = isVisible;

            QuadPyramidPZ.Visible = isVisible;
            QuadPyramidAZ.Visible = isVisible;
            QuadPyramidBZ.Visible = isVisible;
            QuadPyramidCZ.Visible = isVisible;
            QuadPyramidDZ.Visible = isVisible;
        }

        private void VisibleTrianglePyramidParametrs(bool isVisible)
        {
            labelTrianglePyramidP.Visible = isVisible;
            labelTrianglePyramidA.Visible = isVisible;
            labelTrianglePyramidB.Visible = isVisible;
            labelTrianglePyramidC.Visible = isVisible;

            labelTrianglePyramidPX.Visible = isVisible;
            labelTrianglePyramidAX.Visible = isVisible;
            labelTrianglePyramidBX.Visible = isVisible;
            labelTrianglePyramidCX.Visible = isVisible;

            labelTrianglePyramidPY.Visible = isVisible;
            labelTrianglePyramidAY.Visible = isVisible;
            labelTrianglePyramidBY.Visible = isVisible;
            labelTrianglePyramidCY.Visible = isVisible;

            labelTrianglePyramidPZ.Visible = isVisible;
            labelTrianglePyramidAZ.Visible = isVisible;
            labelTrianglePyramidBZ.Visible = isVisible;
            labelTrianglePyramidCZ.Visible = isVisible;

            TrianglePyramidPX.Visible = isVisible;
            TrianglePyramidAX.Visible = isVisible;
            TrianglePyramidBX.Visible = isVisible;
            TrianglePyramidCX.Visible = isVisible;

            TrianglePyramidPY.Visible = isVisible;
            TrianglePyramidAY.Visible = isVisible;
            TrianglePyramidBY.Visible = isVisible;
            TrianglePyramidCY.Visible = isVisible;

            TrianglePyramidPZ.Visible = isVisible;
            TrianglePyramidAZ.Visible = isVisible;
            TrianglePyramidBZ.Visible = isVisible;
            TrianglePyramidCZ.Visible = isVisible;
        }

       

        
        private void buttonAddPrimitive_Click(object sender, EventArgs e)
        {
            if (inputName.Text == "")
            {
                MessageBox.Show("Введите название объекта!");
                return;
            }
            string name = inputName.ToString();
            name = name.Replace("System.Windows.Forms.TextBox, Text: ", "");
            try
            {
                Command checkFreeNamePrimitiveCommand = new CheckFreeNamePrimitiveCommand(name);
                facade.executeCommand(checkFreeNamePrimitiveCommand);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }

            string selectedState = this.selectedAddPrimitive.SelectedItem.ToString();
            Color color = ColorBox.BackColor;
            if (selectedState == "Сфера")
            {
                Command addSphereCommand = new AddSphereCommand(name, new Vec3d((double)SphereCX.Value, (double)SphereCY.Value, (double)SphereCZ.Value), (double)SphereR.Value,
                new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                facade.executeCommand(addSphereCommand);
            }
            else if (selectedState == "Треугольная пирамида")
            {
                Vec3d P = new Vec3d((double)TrianglePyramidPX.Value, (double)TrianglePyramidPY.Value, (double)TrianglePyramidPZ.Value);
                Vec3d A = new Vec3d((double)TrianglePyramidAX.Value, (double)TrianglePyramidAY.Value, (double)TrianglePyramidAZ.Value);
                Vec3d B = new Vec3d((double)TrianglePyramidBX.Value, (double)TrianglePyramidBY.Value, (double)TrianglePyramidBZ.Value);
                Vec3d C = new Vec3d((double)TrianglePyramidCX.Value, (double)TrianglePyramidCY.Value, (double)TrianglePyramidCZ.Value);
                if (checkTrianglePyramid(P, A, B, C) == false)
                {
                    MessageBox.Show("Это не пирамида. Введите другие координаты!");
                    return;
                }
                else
                {
                    Command addTrianglePyramidCommand = new AddTrianglePyramidCommand(name, P, A, B, C,
                    new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                    facade.executeCommand(addTrianglePyramidCommand);
                }
            }
            else if (selectedState == "Четырехугольная пирамида")
            {
                Command addQuadPyramidCommand = new AddQuadPyramidCommand(name, new Vec3d((double)QuadPyramidPX.Value, (double)QuadPyramidPY.Value, (double)QuadPyramidPZ.Value),
                    new Vec3d((double)QuadPyramidAX.Value, (double)QuadPyramidAY.Value, (double)QuadPyramidAZ.Value),
                    new Vec3d((double)QuadPyramidBX.Value, (double)QuadPyramidBY.Value, (double)QuadPyramidBZ.Value),
                    new Vec3d((double)QuadPyramidCX.Value, (double)QuadPyramidCY.Value, (double)QuadPyramidCZ.Value),
                     new Vec3d((double)QuadPyramidDX.Value, (double)QuadPyramidDY.Value, (double)QuadPyramidDZ.Value),
                new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                facade.executeCommand(addQuadPyramidCommand);
            }
            else if (selectedState == "Цилиндр")
            {
                Vec3d tmpV = new Vec3d((double)CylinderVX.Value, (double)CylinderVY.Value, (double)CylinderVZ.Value);
                if (isUnitVector(tmpV) == false)
                {
                    MessageBox.Show("Вектор оси не единичный. Введите другие координаты!");
                    return;
                }
                else
                {
                    Command addCylinderCommand = new AddCylinderCommand(name, new Vec3d((double)CylinderCX.Value, (double)CylinderCY.Value, (double)CylinderCZ.Value),
                                      tmpV,
                    (double)CylinderR.Value, (double)CylinderH.Value, new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                    facade.executeCommand(addCylinderCommand);
                }
            }
            else if (selectedState == "Конус")
            {
                Vec3d tmpV = new Vec3d((double)ConeVX.Value, (double)ConeVY.Value, (double)ConeVZ.Value);
                if (isUnitVector(tmpV) == false)
                {
                    MessageBox.Show("Вектор оси не единичный. Введите другие координаты!");
                    return;
                }
                else
                {
                    Command addConeCommand = new AddConeCommand(name, new Vec3d((double)ConeCX.Value, (double)ConeCY.Value, (double)ConeCZ.Value),
                                      tmpV,
                    (double)ConeK.Value, (double)ConeMinm.Value, (double)ConeMaxm.Value, new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                    facade.executeCommand(addConeCommand);
                }
            }
            else if (selectedState == "Параллелепипед")
            {
                
                Vec3d C = new Vec3d((double)ParalCX.Value, (double)ParalCY.Value, (double)ParalCZ.Value);
                Vec3d E = new Vec3d((double)ParalEX.Value, (double)ParalEY.Value, (double)ParalEZ.Value);
                if (checkParallelepiped(C, E) == false)
                {
                    MessageBox.Show("Это не параллелепипед. Введите другие координаты!");
                    return;
                }
                else
                {
                    Command addParallelepipedCommand = new AddParallelepipedCommand(name, C, E,
                    new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)specular.Value, (double)reflective.Value);
                    facade.executeCommand(addParallelepipedCommand);
                }
            }
            if (checkBox1Render.Checked == true)
                render();
        }


        private void VisibleChoiceSphereParametrs(bool isVisible)
        {
            labelChoiceSphereC.Visible = isVisible;
            labelChoiceSphereR.Visible = isVisible;
            labelChoiceSphereCX.Visible = isVisible;
            labelChoiceSphereCY.Visible = isVisible;
            labelChoiceSphereCZ.Visible = isVisible;

            ChoiceSphereCX.Visible = isVisible;
            ChoiceSphereCY.Visible = isVisible;
            ChoiceSphereCZ.Visible = isVisible;
            ChoiceSphereR.Visible = isVisible;
        }

        private void VisibleChoiceTrianglePyramidParametrs(bool isVisible)
        {
            labelChoiceTrianglePyramidP.Visible = isVisible;
            labelChoiceTrianglePyramidA.Visible = isVisible;
            labelChoiceTrianglePyramidB.Visible = isVisible;
            labelChoiceTrianglePyramidC.Visible = isVisible;

            labelChoiceTrianglePyramidPX.Visible = isVisible;
            labelChoiceTrianglePyramidAX.Visible = isVisible;
            labelChoiceTrianglePyramidBX.Visible = isVisible;
            labelChoiceTrianglePyramidCX.Visible = isVisible;

            labelChoiceTrianglePyramidPY.Visible = isVisible;
            labelChoiceTrianglePyramidAY.Visible = isVisible;
            labelChoiceTrianglePyramidBY.Visible = isVisible;
            labelChoiceTrianglePyramidCY.Visible = isVisible;

            labelChoiceTrianglePyramidPZ.Visible = isVisible;
            labelChoiceTrianglePyramidAZ.Visible = isVisible;
            labelChoiceTrianglePyramidBZ.Visible = isVisible;
            labelChoiceTrianglePyramidCZ.Visible = isVisible;

            ChoiceTrianglePyramidPX.Visible = isVisible;
            ChoiceTrianglePyramidAX.Visible = isVisible;
            ChoiceTrianglePyramidBX.Visible = isVisible;
            ChoiceTrianglePyramidCX.Visible = isVisible;

            ChoiceTrianglePyramidPY.Visible = isVisible;
            ChoiceTrianglePyramidAY.Visible = isVisible;
            ChoiceTrianglePyramidBY.Visible = isVisible;
            ChoiceTrianglePyramidCY.Visible = isVisible;

            ChoiceTrianglePyramidPZ.Visible = isVisible;
            ChoiceTrianglePyramidAZ.Visible = isVisible;
            ChoiceTrianglePyramidBZ.Visible = isVisible;
            ChoiceTrianglePyramidCZ.Visible = isVisible;
        }

        private void VisibleChoiceQuadPyramidParametrs(bool isVisible)
        {
            labelChoiceQuadPyramidP.Visible = isVisible;
            labelChoiceQuadPyramidA.Visible = isVisible;
            labelChoiceQuadPyramidB.Visible = isVisible;
            labelChoiceQuadPyramidC.Visible = isVisible;
            labelChoiceQuadPyramidD.Visible = isVisible;

            labelChoiceQuadPyramidPX.Visible = isVisible;
            labelChoiceQuadPyramidAX.Visible = isVisible;
            labelChoiceQuadPyramidBX.Visible = isVisible;
            labelChoiceQuadPyramidCX.Visible = isVisible;
            labelChoiceQuadPyramidDX.Visible = isVisible;

            labelChoiceQuadPyramidPY.Visible = isVisible;
            labelChoiceQuadPyramidAY.Visible = isVisible;
            labelChoiceQuadPyramidBY.Visible = isVisible;
            labelChoiceQuadPyramidCY.Visible = isVisible;
            labelChoiceQuadPyramidDY.Visible = isVisible;

            labelChoiceQuadPyramidPZ.Visible = isVisible;
            labelChoiceQuadPyramidAZ.Visible = isVisible;
            labelChoiceQuadPyramidBZ.Visible = isVisible;
            labelChoiceQuadPyramidCZ.Visible = isVisible;
            labelChoiceQuadPyramidDZ.Visible = isVisible;

            ChoiceQuadPyramidPX.Visible = isVisible;
            ChoiceQuadPyramidAX.Visible = isVisible;
            ChoiceQuadPyramidBX.Visible = isVisible;
            ChoiceQuadPyramidCX.Visible = isVisible;
            ChoiceQuadPyramidDX.Visible = isVisible;

            ChoiceQuadPyramidPY.Visible = isVisible;
            ChoiceQuadPyramidAY.Visible = isVisible;
            ChoiceQuadPyramidBY.Visible = isVisible;
            ChoiceQuadPyramidCY.Visible = isVisible;
            ChoiceQuadPyramidDY.Visible = isVisible;

            ChoiceQuadPyramidPZ.Visible = isVisible;
            ChoiceQuadPyramidAZ.Visible = isVisible;
            ChoiceQuadPyramidBZ.Visible = isVisible;
            ChoiceQuadPyramidCZ.Visible = isVisible;
            ChoiceQuadPyramidDZ.Visible = isVisible;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                GetSceneObjectsCommand getSceneObjectsCommand = new GetSceneObjectsCommand();
                facade.executeCommand(getSceneObjectsCommand);

                primitives = getSceneObjectsCommand.getResult();

                choiceObject.Items.Clear();
                int flag = 0;
                
                
                for (int i = 0; i < primitives.Count; i++)
                {
                    if (primitives[i] is Triangle)
                        continue;
                    if (primitives[i] is DiskPlane)
                        continue;
                    
                    flag += 1;
                    choiceObject.Items.Add(primitives[i].name);
                    string tmp = (string)choiceObject.Items[flag - 1];
                    tmp = tmp.Replace("System.Windows.Forms.TextBox, Text: ", "");
                    choiceObject.Items[flag - 1] = tmp;
                }
                if (flag > 0)
                {
                    choiceObject.SelectedIndex = 0;
                    
                    ChoiceColor.Visible = true;
                    ChoiceReflective.Visible = true;
                    ChoiceSpecular.Visible = true;
                    ChoiceName.Visible = true;
                    labelChoiceReflective.Visible = true;
                    labelChoiceSpecular.Visible = true;
                    btnChange.Visible = true;
                    checkBox2Render.Visible = true;

                    string selectedObject = this.choiceObject.SelectedItem.ToString();
                    if (selectedObject == "плоскость основания")
                    {
                        btnDeletePrimitive.Visible = false;
                        ChoiceName.Visible = false;
                        labelChoiceName.Visible = false;
                    }
                    else
                    {
                        btnDeletePrimitive.Visible = true;
                        ChoiceName.Visible = true;
                        labelChoiceName.Visible = true;
                    }
                }
                
                else
                {
                    VisibleChoiceSphereParametrs(false);
                    VisibleChoiceTrianglePyramidParametrs(false);
                    VisibleChoiceQuadPyramidParametrs(false);
                    VisibleChoiceCylinderParametrs(false);
                    VisibleChoiceConeParametrs(false);
                    VisibleChoiceParallelepipedParametrs(false);
                    VisibleVParametrs(false);
                    ChoiceColor.Visible = false;
                    ChoiceReflective.Visible = false;
                    ChoiceSpecular.Visible = false;
                    ChoiceName.Visible = false;
                    labelChoiceName.Visible = false;
                    labelChoiceColor.Visible = false;
                    labelChoiceReflective.Visible = false;
                    labelChoiceSpecular.Visible = false;
                    btnChange.Visible = false;
                    btnDeletePrimitive.Visible = false;
                    checkBox2Render.Visible = false;


                }    
                
            }
            if (tabControl1.SelectedIndex == 2)
            {
                GetSceneObjectsCommand getSceneObjectsCommand = new GetSceneObjectsCommand();
                facade.executeCommand(getSceneObjectsCommand);

                primitives = getSceneObjectsCommand.getResult();
                listBox1.Items.Clear();
                string str = "";
                int flag;
                for (int i = 0; i < primitives.Count; i++)
                {
                    flag = 0;
                    if (primitives[i] is Sphere)
                    {
                        Sphere tmp = (Sphere)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Сфера, ";
                        str += "Parametrs: C = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "R = " + tmp.radius + ", ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is Cylinder)
                    {
                        Cylinder tmp = (Cylinder)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Цилиндр, ";
                        str += "Parametrs: Cосн = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "Rосн = " + tmp.radius + ", ";
                        str += "V = (" + String.Format("{0:f6}", tmp.V.x) + "; " + String.Format("{0:f6}", tmp.V.y) + "; " + String.Format("{0:f6}", tmp.V.z) + "), ";
                        str += "H = " + tmp.maxm + ", ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is Cone)
                    {
                        Cone tmp = (Cone)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Конус, ";
                        str += "Parametrs: Cосн = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "V = (" + String.Format("{0:f6}", tmp.V.x) + "; " + String.Format("{0:f6}", tmp.V.y) + "; " + String.Format("{0:f6}", tmp.V.z) + "), ";
                        str += "Minm = " + tmp.minm + ", ";
                        str += "Maxm = " + tmp.maxm + ", ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is Parallelepiped)
                    {
                        Parallelepiped tmp = (Parallelepiped)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Параллелепипед, ";
                        str += "Parametrs: С = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "E = (" + tmp.E.x + "; " + tmp.E.y + "; " + tmp.E.z + "), ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is TrianglePyramid)
                    {
                        TrianglePyramid tmp = (TrianglePyramid)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Пирамида, ";
                        str += "Parametrs: P = (" + tmp.P.x + "; " + tmp.P.y + "; " + tmp.P.z + "), ";
                        str += "A = (" + tmp.A.x + "; " + tmp.A.y + "; " + tmp.A.z + "), ";
                        str += "B = (" + tmp.B.x + "; " + tmp.B.y + "; " + tmp.B.z + "), ";
                        str += "C = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is QuadPyramid)
                    {
                        QuadPyramid tmp = (QuadPyramid)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "Type: Пирамида, ";
                        str += "Parametrs: P = (" + tmp.P.x + "; " + tmp.P.y + "; " + tmp.P.z + "), ";
                        str += "A = (" + tmp.A.x + "; " + tmp.A.y + "; " + tmp.A.z + "), ";
                        str += "B = (" + tmp.B.x + "; " + tmp.B.y + "; " + tmp.B.z + "), ";
                        str += "C = (" + tmp.C.x + "; " + tmp.C.y + "; " + tmp.C.z + "), ";
                        str += "D = (" + tmp.D.x + "; " + tmp.D.y + "; " + tmp.D.z + "), ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    else if (primitives[i] is Plane)
                    {
                        Plane tmp = (Plane)primitives[i];
                        str = "Name: " + tmp.name + ", ";
                        str += "color = (" + tmp.color.x + "; " + tmp.color.y + "; " + tmp.color.z + "), ";
                        str += "specular = " + tmp.specular + ", ";
                        str += "reflective = " + tmp.reflective;
                        flag = 1;
                    }
                    if (flag == 1)
                        listBox1.Items.Add(str);

                }

            }
        }

        private void buttonRollCamera_Click(object sender, EventArgs e)
        {
            if ((double)rollCamera.Value != 360 || (double)rollCamera.Value != -360)
            {
                Command rollCameraCommand = new RollCameraCommand((double)rollCamera.Value);
                facade.executeCommand(rollCameraCommand);

                render();
            }
            GetCameraCommand getCameraCommand = new GetCameraCommand();
            facade.executeCommand(getCameraCommand);
            Camera camera = getCameraCommand.getResult();


            CameraRotOZ.Text = camera.angle.z.ToString();
        }

        private void buttonYawCamera_Click(object sender, EventArgs e)
        {
            if ((double)yawCamera.Value != 360 || (double)yawCamera.Value != -360)
            {
                Command yawCameraCommand = new YawCameraCommand((double)yawCamera.Value);
                facade.executeCommand(yawCameraCommand);

                render();
            }
            GetCameraCommand getCameraCommand = new GetCameraCommand();
            facade.executeCommand(getCameraCommand);
            Camera camera = getCameraCommand.getResult();


            CameraRotOX.Text = camera.angle.x.ToString();
        }

        private void buttonPitchCamera_Click(object sender, EventArgs e)
        {
            if ((double)pitchCamera.Value != 360 || (double)pitchCamera.Value != -360)
            {
                Command pitchCameraCommand = new PitchCameraCommand((double)pitchCamera.Value);
                facade.executeCommand(pitchCameraCommand);

                render();
            }
            GetCameraCommand getCameraCommand = new GetCameraCommand();
            facade.executeCommand(getCameraCommand);
            Camera camera = getCameraCommand.getResult();
            
            
            CameraRotOY.Text = camera.angle.y.ToString();
            
        }

        private void buttonMoveCamera_Click(object sender, EventArgs e)
        {

            Command moveCameraCommand = new MoveCameraCommand((double)moveCameraDX.Value, (double)moveCameraDY.Value, (double)moveCameraDZ.Value);
            facade.executeCommand(moveCameraCommand);

            
            render();
            GetCameraCommand getCameraCommand = new GetCameraCommand();
            facade.executeCommand(getCameraCommand);
            Camera camera = getCameraCommand.getResult();
            CameraPosX.Text = camera.position.x.ToString();
            CameraPosY.Text = camera.position.y.ToString();
            CameraPosZ.Text = camera.position.z.ToString();

        }



        private void VisibleChoiceConeParametrs(bool isVisible)
        {
            labelChoiceConeC.Visible = isVisible;
            labelChoiceConeK.Visible = isVisible;
            labelChoiceConeFrom.Visible = isVisible;
            labelChoiceConeTo.Visible = isVisible;
            labelChoiceConeGrad.Visible = isVisible;
            labelChoiceV.Visible = isVisible;
            labelChoiceConeCX.Visible = isVisible;
            labelChoiceConeCY.Visible = isVisible;
            labelChoiceConeCZ.Visible = isVisible;
            labelChoiceCylinderVX.Visible = isVisible;
            labelChoiceCylinderVY.Visible = isVisible;
            labelChoiceCylinderVZ.Visible = isVisible;
            labelChoiceConeH.Visible = isVisible;


            ChoiceConeCX.Visible = isVisible;
            ChoiceConeCY.Visible = isVisible;
            ChoiceConeCZ.Visible = isVisible;

            ChoiceVX.Visible = isVisible;
            ChoiceVY.Visible = isVisible;
            ChoiceVZ.Visible = isVisible;

            ChoiceConeMaxm.Visible = isVisible;
            ChoiceConeMinm.Visible = isVisible;
            ChoiceConeK.Visible = isVisible;




        }

        private void VisibleChoiceCylinderParametrs(bool isVisible)
        {
             labelChoiceCylinderC.Visible = isVisible;
             labelChoiceCylinderR.Visible = isVisible;
             labelChoiceV.Visible = isVisible;
             labelChoiceCylinderCX.Visible = isVisible;
             labelChoiceCylinderCY.Visible = isVisible;
             labelChoiceCylinderCZ.Visible = isVisible;
             labelChoiceCylinderVX.Visible = isVisible;
             labelChoiceCylinderVY.Visible = isVisible;
             labelChoiceCylinderVZ.Visible = isVisible;

            labelChoiceCylinderH.Visible = isVisible;

             ChoiceCylinderCX.Visible = isVisible;
             ChoiceCylinderCY.Visible = isVisible;
             ChoiceCylinderCZ.Visible = isVisible;

            ChoiceVX.Visible = isVisible;
            ChoiceVY.Visible = isVisible;
            ChoiceVZ.Visible = isVisible;

            ChoiceCylinderR.Visible = isVisible;
             ChoiceCylinderH.Visible = isVisible;
        }

        private void VisibleVParametrs(bool isVisible)
        {
            

            labelWorkV.Visible = isVisible;
            labelRotV.Visible = isVisible;
            labelRotVX.Visible = isVisible;
            labelRotVY.Visible = isVisible;
            labelRotVZ.Visible = isVisible;
            RotVX.Visible = isVisible;
            RotVY.Visible = isVisible;
            RotVZ.Visible = isVisible;
            labelChangeVX.Visible = isVisible;
            labelChangeVY.Visible = isVisible;
            labelChangeVZ.Visible = isVisible;
            ChangeVX.Visible = isVisible;
            ChangeVY.Visible = isVisible;
            ChangeVZ.Visible = isVisible;

            btnChangeV.Visible = isVisible;
            labelChangeV.Visible = isVisible;

            btnRotVX.Visible = isVisible;
            btnRotVY.Visible = isVisible;
            btnRotVZ.Visible = isVisible;

        }

        private void choiceObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (primitives.Count == 0)
                return;
            string selectedObject = this.choiceObject.SelectedItem.ToString();
            for (int i = 0; i < primitives.Count; i++)
            {
                if (primitives[i] is DiskPlane)
                    continue;
                if (primitives[i] is Triangle)
                    continue;

                if (primitives[i].name == selectedObject )
                {
                    primitive = primitives[i];
                    break;
                }
            }
            if (primitive is Sphere)
            {
                Sphere tmp = (Sphere)primitive;
                VisibleChoiceSphereParametrs(true);
                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleVParametrs(false);

                ChoiceName.Text = tmp.name;
                ChoiceName.Visible = true;
                labelChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                ChoiceSphereCX.Value = (decimal)tmp.C.x;
                ChoiceSphereCY.Value = (decimal)tmp.C.y;
                ChoiceSphereCZ.Value = (decimal)tmp.C.z;
                ChoiceSphereR.Value = (decimal)tmp.radius;
                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);


            }
            else if (primitive is TrianglePyramid)
            {

                VisibleChoiceTrianglePyramidParametrs(true);
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleVParametrs(false);
                TrianglePyramid tmp = (TrianglePyramid)primitive;
                ChoiceName.Text = tmp.name;
                labelChoiceName.Visible = true;
                ChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                ChoiceTrianglePyramidPX.Value = (decimal)tmp.P.x;
                ChoiceTrianglePyramidPY.Value = (decimal)tmp.P.y;
                ChoiceTrianglePyramidPZ.Value = (decimal)tmp.P.z;

                ChoiceTrianglePyramidAX.Value = (decimal)tmp.A.x;
                ChoiceTrianglePyramidAY.Value = (decimal)tmp.A.y;
                ChoiceTrianglePyramidAZ.Value = (decimal)tmp.A.z;

                ChoiceTrianglePyramidBX.Value = (decimal)tmp.B.x;
                ChoiceTrianglePyramidBY.Value = (decimal)tmp.B.y;
                ChoiceTrianglePyramidBZ.Value = (decimal)tmp.B.z;

                ChoiceTrianglePyramidCX.Value = (decimal)tmp.C.x;
                ChoiceTrianglePyramidCY.Value = (decimal)tmp.C.y;
                ChoiceTrianglePyramidCZ.Value = (decimal)tmp.C.z;

                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);

            }
            else if (primitive is QuadPyramid)
            {

                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceQuadPyramidParametrs(true);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleVParametrs(false);
                QuadPyramid tmp = (QuadPyramid)primitive;
                ChoiceName.Text = tmp.name;
                labelChoiceName.Visible = true;
                ChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                ChoiceQuadPyramidPX.Value = (decimal)tmp.P.x;
                ChoiceQuadPyramidPY.Value = (decimal)tmp.P.y;
                ChoiceQuadPyramidPZ.Value = (decimal)tmp.P.z;

                ChoiceQuadPyramidAX.Value = (decimal)tmp.A.x;
                ChoiceQuadPyramidAY.Value = (decimal)tmp.A.y;
                ChoiceQuadPyramidAZ.Value = (decimal)tmp.A.z;

                ChoiceQuadPyramidBX.Value = (decimal)tmp.B.x;
                ChoiceQuadPyramidBY.Value = (decimal)tmp.B.y;
                ChoiceQuadPyramidBZ.Value = (decimal)tmp.B.z;

                ChoiceQuadPyramidCX.Value = (decimal)tmp.C.x;
                ChoiceQuadPyramidCY.Value = (decimal)tmp.C.y;
                ChoiceQuadPyramidCZ.Value = (decimal)tmp.C.z;

                ChoiceQuadPyramidDX.Value = (decimal)tmp.D.x;
                ChoiceQuadPyramidDY.Value = (decimal)tmp.D.y;
                ChoiceQuadPyramidDZ.Value = (decimal)tmp.D.z;

                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);

            }
            else if (primitive is Cylinder)
            {

                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceCylinderParametrs(true);
                VisibleVParametrs(true);
                Cylinder tmp = (Cylinder)primitive;
                V = tmp.V;
                labelChoiceName.Visible = true;
                ChoiceName.Text = tmp.name;
                ChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                labelChoiceV.Text = "Вектор оси цилиндра";
                labelWorkV.Text = "Действия с вектором оси цилиндра :";
                ChoiceCylinderCX.Value = (decimal)tmp.C.x;
                ChoiceCylinderCY.Value = (decimal)tmp.C.y;
                ChoiceCylinderCZ.Value = (decimal)tmp.C.z;

                ChoiceVX.Text = String.Format("{0:f6}", tmp.V.x);
                ChoiceVY.Text = String.Format("{0:f6}", tmp.V.y);
                ChoiceVZ.Text = String.Format("{0:f6}", tmp.V.z);

                ChoiceCylinderH.Value = (decimal)tmp.maxm;
                ChoiceCylinderR.Value = (decimal)tmp.radius;

                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);
            }
            else if (primitive is Cone)
            {

                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceConeParametrs(true);
                VisibleVParametrs(true);
                Cone tmp = (Cone)primitive;
                V = tmp.V;
                labelChoiceName.Visible = true;
                ChoiceName.Text = tmp.name;
                ChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                labelChoiceV.Text = "Вектор оси конуса";
                labelWorkV.Text = "Действия с вектором оси конуса :";
                ChoiceConeCX.Value = (decimal)tmp.C.x;
                ChoiceConeCY.Value = (decimal)tmp.C.y;
                ChoiceConeCZ.Value = (decimal)tmp.C.z;

                ChoiceVX.Text = String.Format("{0:f6}", tmp.V.x);
                ChoiceVY.Text = String.Format("{0:f6}", tmp.V.y);
                ChoiceVZ.Text = String.Format("{0:f6}", tmp.V.z);

                ChoiceConeK.Value = (decimal)tmp.angle;
                ChoiceConeMaxm.Value = (decimal)tmp.maxm;
                ChoiceConeMinm.Value = (decimal)tmp.minm;

                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);
            }
            else if (primitive is Parallelepiped)
            {
                Parallelepiped tmp = (Parallelepiped)primitive;
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(true);
                VisibleChoiceCylinderParametrs(false);
                VisibleVParametrs(false);

                ChoiceName.Text = tmp.name;
                labelChoiceName.Visible = true;
                ChoiceName.Visible = true;
                btnDeletePrimitive.Visible = true;
                ChoiceParalCX.Value = (decimal)tmp.C.x;
                ChoiceParalCY.Value = (decimal)tmp.C.y;
                ChoiceParalCZ.Value = (decimal)tmp.C.z;
                ChoiceParalEX.Value = (decimal)tmp.E.x;
                ChoiceParalEY.Value = (decimal)tmp.E.y;
                ChoiceParalEZ.Value = (decimal)tmp.E.z;
                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);


            }
            else if (primitive is Plane)
            {
                Plane tmp = (Plane)primitive;
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleVParametrs(false);
                labelChoiceName.Visible = false;
                ChoiceName.Visible = false;
                btnDeletePrimitive.Visible = false;
                ChoiceSpecular.Value = (decimal)tmp.specular;
                ChoiceReflective.Value = (decimal)tmp.reflective;
                ChoiceColor.BackColor = Color.FromArgb((int)tmp.color.x, (int)tmp.color.y, (int)tmp.color.z);


            }
            else
            {
                VisibleChoiceSphereParametrs(false);
                VisibleChoiceTrianglePyramidParametrs(false);
                VisibleChoiceQuadPyramidParametrs(false);
                VisibleChoiceCylinderParametrs(false);
                VisibleChoiceConeParametrs(false);
                VisibleChoiceParallelepipedParametrs(false);
                VisibleVParametrs(false);
            }
        }

        private void VisibleLightParametrs(bool isVisible)
        {
            labelLightPos.Visible = isVisible;
            labelChangeLightPos.Visible = isVisible;

            LightPosX.Visible = isVisible;
            ChangeLightPosX.Visible = isVisible;

            LightPosY.Visible = isVisible;
            ChangeLightPosY.Visible = isVisible;

            LightPosZ.Visible = isVisible;
            ChangeLightPosZ.Visible = isVisible;
        }

        private void comboBoxLights_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLight = this.comboBoxLights.SelectedItem.ToString();
            GetLightCommand getLightCommand = new GetLightCommand(selectedLight);
            facade.executeCommand(getLightCommand);

            Light light = getLightCommand.getResult();

            if (comboBoxLights.SelectedIndex == 0)
            {
                VisibleLightParametrs(false);
                btnDeleteLight.Visible = false;
            }
            else
            {
                VisibleLightParametrs(true);
                if (comboBoxLights.SelectedIndex == 1)
                {
                    btnDeleteLight.Visible = false;
                    labelLightPos.Text = "Направление:";
                    labelChangeLightPos.Text = "Направление:";

                }
                else
                {
                    btnDeleteLight.Visible = true;
                    labelLightPos.Text = "Положение:";
                    labelChangeLightPos.Text = "Положение:";
                }
                LightPosX.Text = light.position.x.ToString();
                LightPosY.Text = light.position.y.ToString();
                LightPosZ.Text = light.position.z.ToString();

                ChangeLightPosX.Value = (decimal)light.position.x;
                ChangeLightPosY.Value = (decimal)light.position.y;
                ChangeLightPosZ.Value = (decimal)light.position.z;

            }
            LightIntensity.Text = light.intensity.ToString();
            ChangeLightIntensity.Value = (decimal)light.intensity;

        }

        private void UpdateLightsName()
        {
            GetLightsNameCommand getLightsNameCommand = new GetLightsNameCommand();
            facade.executeCommand(getLightsNameCommand);
            comboBoxLights.Items.Clear();
            List<string> lightsName = getLightsNameCommand.getResult();
            for (int i = 0; i < lightsName.Count; i++)
                comboBoxLights.Items.Add(lightsName[i]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "3DConstructor";
            UpdateLightsName();
            comboBoxLights.SelectedIndex = 1;
            selectedAddPrimitive.SelectedIndex = 0;
            CameraPosX.Text = "0";
            CameraPosY.Text = "0";
            CameraPosZ.Text = "0";
            CameraRotOX.Text = "0";
            CameraRotOY.Text = "0";
            CameraRotOZ.Text = "0";


        }

        private void btnChangeLightParams_Click(object sender, EventArgs e)
        {
            string selectedLight = this.comboBoxLights.SelectedItem.ToString();
            ChangeLightCommand changeLightCommand;
            if (comboBoxLights.SelectedIndex == 0)
            {
                VisibleLightParametrs(false);
                changeLightCommand = new ChangeLightCommand(selectedLight, (double)ChangeLightIntensity.Value);
            }
            else
            {
                changeLightCommand = new ChangeLightCommand(selectedLight,
                   new Vec3d((double)ChangeLightPosX.Value, (double)ChangeLightPosY.Value, (double)ChangeLightPosZ.Value), (double)ChangeLightIntensity.Value);
            }
            facade.executeCommand(changeLightCommand);
            render();
            LightIntensity.Text = ChangeLightIntensity.Value.ToString();
            LightPosX.Text = ChangeLightPosX.Value.ToString();
            LightPosY.Text = ChangeLightPosY.Value.ToString();
            LightPosZ.Text = ChangeLightPosZ.Value.ToString();
        }

        private void btnAddLight_Click(object sender, EventArgs e)
        {
            AddLightCommand addLightCommand = new AddLightCommand(new Vec3d((double)AddLightPosX.Value, (double)AddLightPosY.Value, (double)AddLightPosZ.Value), (double)AddLightIntensity.Value);
            facade.executeCommand(addLightCommand);
            render();
            UpdateLightsName();
            comboBoxLights.SelectedIndex = 1;
        }

        private void btnDeleteLight_Click(object sender, EventArgs e)
        {
            string selectedLight = this.comboBoxLights.SelectedItem.ToString();
            DeleteLightCommand deleteLightCommand = new DeleteLightCommand(selectedLight);
            facade.executeCommand(deleteLightCommand);
            render();
            UpdateLightsName();
            comboBoxLights.SelectedIndex = 1;
        }

        private void ChoiceColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            ChoiceColor.BackColor = colorDialog1.Color;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (primitive is Sphere)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                UpdateSphereCommand updateSphereCommand = new UpdateSphereCommand(primitive.name, choiceName,
                    new Vec3d((double)ChoiceSphereCX.Value, (double)ChoiceSphereCY.Value, (double)ChoiceSphereCZ.Value),
                    (double)ChoiceSphereR.Value, new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);

                facade.executeCommand(updateSphereCommand);
            }
            else if (primitive is TrianglePyramid)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                Vec3d P = new Vec3d((double)ChoiceTrianglePyramidPX.Value, (double)ChoiceTrianglePyramidPY.Value, (double)ChoiceTrianglePyramidPZ.Value);
                Vec3d A = new Vec3d((double)ChoiceTrianglePyramidAX.Value, (double)ChoiceTrianglePyramidAY.Value, (double)ChoiceTrianglePyramidAZ.Value);
                Vec3d B = new Vec3d((double)ChoiceTrianglePyramidBX.Value, (double)ChoiceTrianglePyramidBY.Value, (double)ChoiceTrianglePyramidBZ.Value);
                Vec3d C = new Vec3d((double)ChoiceTrianglePyramidCX.Value, (double)ChoiceTrianglePyramidCY.Value, (double)ChoiceTrianglePyramidCZ.Value);
                if (checkTrianglePyramid(P, A, B, C) == false)
                {
                    MessageBox.Show("Это не пирамида. Введите другие координаты!");
                    return;
                }
                else
                {
                    UpdateTrianglePyramidCommand updateTrianglePyramidCommand = new UpdateTrianglePyramidCommand(primitive.name, choiceName,
                        P, A, B, C,

                       new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);
                    facade.executeCommand(updateTrianglePyramidCommand);
                }


            }
            else if (primitive is QuadPyramid)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                UpdateQuadPyramidCommand updateQuadPyramidCommand = new UpdateQuadPyramidCommand(primitive.name, choiceName,
                    new Vec3d((double)ChoiceQuadPyramidPX.Value, (double)ChoiceQuadPyramidPY.Value, (double)ChoiceQuadPyramidPZ.Value),
                    new Vec3d((double)ChoiceQuadPyramidAX.Value, (double)ChoiceQuadPyramidAY.Value, (double)ChoiceQuadPyramidAZ.Value),
                    new Vec3d((double)ChoiceQuadPyramidBX.Value, (double)ChoiceQuadPyramidBY.Value, (double)ChoiceQuadPyramidBZ.Value),
                    new Vec3d((double)ChoiceQuadPyramidCX.Value, (double)ChoiceQuadPyramidCY.Value, (double)ChoiceQuadPyramidCZ.Value),
                    new Vec3d((double)ChoiceQuadPyramidDX.Value, (double)ChoiceQuadPyramidDY.Value, (double)ChoiceQuadPyramidDZ.Value),

                   new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);

                facade.executeCommand(updateQuadPyramidCommand);
            }
            else if (primitive is Cylinder)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                UpdateCylinderCommand updateCylinderCommand = new UpdateCylinderCommand(primitive.name, choiceName,
                    new Vec3d((double)ChoiceCylinderCX.Value, (double)ChoiceCylinderCY.Value, (double)ChoiceCylinderCZ.Value),
                    V, (double)ChoiceCylinderR.Value, (double)ChoiceCylinderH.Value,
                   new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);

                facade.executeCommand(updateCylinderCommand);
            }
            else if (primitive is Cone)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                UpdateConeCommand updateConeCommand = new UpdateConeCommand(primitive.name, choiceName,
                    new Vec3d((double)ChoiceConeCX.Value, (double)ChoiceConeCY.Value, (double)ChoiceConeCZ.Value),
                    V, (double)ChoiceConeK.Value, (double)ChoiceConeMinm.Value, (double)ChoiceConeMaxm.Value,
                   new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);

                facade.executeCommand(updateConeCommand);
            }
            else if (primitive is Parallelepiped)
            {
                string choiceName = ChoiceName.ToString();
                choiceName = choiceName.Replace("System.Windows.Forms.TextBox, Text: ", "");
                Color color = ChoiceColor.BackColor;
                Vec3d C = new Vec3d((double)ChoiceParalCX.Value, (double)ChoiceParalCY.Value, (double)ChoiceParalCZ.Value);
                Vec3d E = new Vec3d((double)ChoiceParalEX.Value, (double)ChoiceParalEY.Value, (double)ChoiceParalEZ.Value);
                if (checkParallelepiped(C, E) == false)
                {
                    MessageBox.Show("Это не параллелепипед. Введите другие координаты!");
                    return;
                }
                else
                {
                    UpdateParallelepipedCommand updateParallelepipedCommand = new UpdateParallelepipedCommand(primitive.name, choiceName,
                        C, E,
                       new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);

                    facade.executeCommand(updateParallelepipedCommand);
                }
            }
            else if (primitive is Plane && primitive.name == "плоскость основания")
            {
                Color color = ChoiceColor.BackColor;
                UpdateBasePlaneCommand updateBasePlaneCommand = new UpdateBasePlaneCommand(new Vec3d((double)color.R, (double)color.G, (double)color.B), (double)ChoiceSpecular.Value, (double)ChoiceReflective.Value);
                facade.executeCommand(updateBasePlaneCommand);
            }
            if (checkBox2Render.Checked == true)
                render();
        }


        private void selectedAddPrimitive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = this.selectedAddPrimitive.SelectedItem.ToString();
            if (selectedState == "Сфера")
            {
                VisibleSphereParametrs(true);
                VisibleCylinderParametrs(false);
                VisibleConeParametrs(false);
                VisibleParallelipipedParametrs(false);
                VisibleTrianglePyramidParametrs(false);
                VisibleQuadPyramidParametrs(false);
            }
            else if (selectedState == "Треугольная пирамида")
            {
                VisibleSphereParametrs(false);
                VisibleCylinderParametrs(false);
                VisibleConeParametrs(false);
                VisibleParallelipipedParametrs(false);
                VisibleTrianglePyramidParametrs(true);
                VisibleQuadPyramidParametrs(false);
            }
            else if (selectedState == "Четырехугольная пирамида")
            {
                VisibleSphereParametrs(false);
                VisibleCylinderParametrs(false);
                VisibleConeParametrs(false);
                VisibleParallelipipedParametrs(false);
                VisibleTrianglePyramidParametrs(false);
                VisibleQuadPyramidParametrs(true);
            }
            else if (selectedState == "Цилиндр")
            {
                VisibleSphereParametrs(false);
                VisibleCylinderParametrs(true);
                VisibleConeParametrs(false);
                VisibleParallelipipedParametrs(false);
                VisibleTrianglePyramidParametrs(false);
                VisibleQuadPyramidParametrs(false);
            }
            else if (selectedState == "Конус")
            {
                VisibleSphereParametrs(false);
                VisibleCylinderParametrs(false);
                VisibleConeParametrs(true);
                VisibleParallelipipedParametrs(false);
                VisibleTrianglePyramidParametrs(false);
                VisibleQuadPyramidParametrs(false);
            }
            else if (selectedState == "Параллелепипед")
            {
                VisibleSphereParametrs(false);
                VisibleCylinderParametrs(false);
                VisibleConeParametrs(false);
                VisibleParallelipipedParametrs(true);
                VisibleTrianglePyramidParametrs(false);
                VisibleQuadPyramidParametrs(false);
            }
        }


        private void MultiplyMV(double[,] matr)
        {
            double[] tmp = new double[4] { V.x, V.y, V.z, 1 };
            double[] result = new double[4] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i] += tmp[j] * matr[i, j];
                }
            }

            V.x = result[0];
            V.y = result[1];
            V.z = result[2];
        }

        private void btnRotVX_Click(object sender, EventArgs e)
        {
            double angle = (double)RotVX.Value;
            double[,] RMtr = new double[4, 4] { { 1, 0, 0, 0 },
                                                { 0, Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180), 0 },
                                                { 0, Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180), 0 },
                                                { 0, 0, 0, 1 } };
            MultiplyMV(RMtr);
            updateLabelV();
        }

        private void btnRotVY_Click(object sender, EventArgs e)
        {
            double angle = (double)RotVY.Value;
            double[,] RMtr = new double[4, 4] { { Math.Cos(angle * Math.PI / 180), 0, -Math.Sin(angle * Math.PI / 180), 0 },
                                                { 0, 1, 0, 0 },
                                                { Math.Sin(angle * Math.PI / 180), 0, Math.Cos(angle * Math.PI / 180), 0 },
                                                { 0, 0, 0, 1 } };
            MultiplyMV(RMtr);
            updateLabelV();
        }

        private void btnRotVZ_Click(object sender, EventArgs e)
        {
            double angle = (double)RotVZ.Value;
            double[,] RMtr = new double[4, 4] { { Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180), 0, 0 },
                                                { Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180), 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 0, 1 } };
            MultiplyMV(RMtr);
            updateLabelV();
        }

        private void btnChangeV_Click(object sender, EventArgs e)
        {
            Vec3d tmpV = new Vec3d((double)ChangeVX.Value, (double)ChangeVY.Value, (double)ChangeVZ.Value);
            if (isUnitVector(tmpV) == false)
            {
                MessageBox.Show("Вектор оси не единичный. Введите другие координаты!");
                return;
            }
            V = tmpV;
            updateLabelV();
        }

        private void updateLabelV()
        {
            ChoiceVX.Text = String.Format("{0:f6}", V.x);
            ChoiceVY.Text = String.Format("{0:f6}", V.y);
            ChoiceVZ.Text = String.Format("{0:f6}", V.z);
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            render();
        }

        private void btnDeletePrimitive_Click(object sender, EventArgs e)
        {
            string selectedObject = this.choiceObject.SelectedItem.ToString();
            Command deletePrimitive = new DeletePrimitiveCommand(selectedObject);
            facade.executeCommand(deletePrimitive);
            tabControl1_SelectedIndexChanged(sender, e);
            render();
            
        }

        private bool checkTrianglePyramid(Vec3d P, Vec3d A, Vec3d B, Vec3d C)
        {
            Vec3d PA = P - A;
            Vec3d PB = P - B;
            Vec3d PC = P - C;

            double V = Math.Abs(PA.x * (PB.y * PC.z - PB.z * PC.y) - PA.y * (PB.x * PC.z - PB.z * PC.x) + PA.z * (PB.x * PC.y - PB.y * PC.x));

            if (V < 10e-8)
                return false;
            return true;
        }
        private bool checkParallelepiped(Vec3d C, Vec3d E)
        {
         
            if (C.x < E.x && C.y < E.y && C.z < E.z)
                return true;
            return false;
        }

        private bool isUnitVector(Vec3d vec)
        {
            if (Vec3d.Length(vec) == 1)
                return true;
            return false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Command drawAxes;
            if (checkBox1.Checked == true)
                drawAxes = new DrawAxesCommand(ref canvas, true);
            else
                drawAxes = new DrawAxesCommand(ref canvas, false);
            facade.executeCommand(drawAxes);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Command clearScene = new ClearSceneCommand();
            facade.executeCommand(clearScene);
            tabControl1_SelectedIndexChanged(sender, e);
            render();
        }
    }
}
