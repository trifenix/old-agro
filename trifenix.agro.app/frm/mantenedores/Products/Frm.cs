﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using trifenix.agro.app.helper;
using trifenix.agro.app.interfaces;
using trifenix.agro.db;
using trifenix.connect.agro.index_model.enums;
using trifenix.connect.agro.index_model.props;
using trifenix.connect.agro.model_input;
using trifenix.connect.agro.resources;
using trifenix.connect.agro_model;
using trifenix.connect.agro_model_input;
using trifenix.connect.app.cloud;

namespace trifenix.agro.app.frm.mantenedores.product
{
    public partial class Frm : Form, IFenixForm
    {
        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Product, ProductInput>();
        });

        BackgroundWorker bworker;

        private string entityName;

        public CurrentFormState State { get; set; }

        public bool Loading { get; set; } = false;

        private bool _addinNew = false;
        public Frm()
        {
            InitializeComponent();
            entityName = GetEntityName();
            pb.Maximum = 100;
            pb.Step = 1;
            bworker = new BackgroundWorker();

            bworker.WorkerReportsProgress = true;

            lblDescripcion.Text = Description();


        }
        private async void SectorFrm_Load_1(object sender, EventArgs e)
        {
            
            
            
            

            SetElements();

            
            
        }
        public void SetElements()
        {
            pb.Visible = true;
            lblProgress.Text = "40%";
            pb.Value = 40;
            var lst = new List<ProductInput>();
            lst.AddRange(GetList() as IEnumerable<ProductInput>);
            var bl = new BindingList<ProductInput>(lst);
            bl.AllowNew = true;
            bl.AllowEdit = true;
            bl.AllowRemove = true;
            

            bsMain.DataSource = bl;

            
            if (bsMain.Count != 0)
            {
                gbxItem.Visible = true;
                gbxItem.Enabled = false;
                pnlButtons.Enabled = true;
                btnEditSector.Enabled = true;
                btnDeleteSector.Enabled = true;
            }
            else
            {
                pnlButtons.Enabled = true;
                btnEditSector.Enabled = false;
                btnDeleteSector.Enabled = false;
            }

            var bnames = Cloud.GetElements<Ingredient>(EntityRelated.INGREDIENT);
            bsIngredient.DataSource = bnames;

            pb.Value = 100;
            lblProgress.Text = "100%";
            pb.Visible = false;
            lblProgress.Visible = false;
            Loading = false;
            cbKindMeasure.DataSource = Enum.GetNames(typeof(MeasureType)).
                Select(o => new { Text = o, Value = (MeasureType)(Enum.Parse(typeof(MeasureType), o)) }).ToList();
            cbKindMeasure.DisplayMember = "Text";
            cbKindMeasure.ValueMember = "Value";

            cbKindContainer.DataSource = Enum.GetNames(typeof(KindOfProductContainer)).
                Select(o => new { Text = o, Value = (KindOfProductContainer)(Enum.Parse(typeof(KindOfProductContainer), o)) }).ToList();
            cbKindContainer.DisplayMember = "Text";
            cbKindContainer.ValueMember = "Value";





        }


        

        private void OnAdd()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            
            State = CurrentFormState.NEW;
            pnlButtons.Enabled = false;
            pnlDoses.Enabled = true;
            bsMain.AddNew();
            var input = (ProductInput)bsMain.Current;
            input.Doses = Array.Empty<DosesInput>();
            

            
        }

        private void OnEdit()
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = true;
            gbxItem.Text = $"Nuevo {FriendlyName()}";
            State = CurrentFormState.EDIT;
            pnlButtons.Enabled = false;

        }

        private void OnCurrentChange() {
            gbxItem.Visible = true;
            gbxItem.Enabled = false;
            pnlButtons.Enabled = !Loading;
        }

        
        

        






        private void tbxName_Validated(object sender, EventArgs e)
        {
            
        }

        


        

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Valida())
            {
               
                return;
            }
            LoadProgress(DoWork);

            

        }

        private void LoadProgress(Action action) {
            bworker.ProgressChanged += bworker_ProgressChanged;

            bworker.DoWork += Bworker_DoWork;

            bworker.RunWorkerAsync(action);

            bworker.WorkerReportsProgress = true;

            Task.Run(() =>
            {
                bworker.ReportProgress(0);
            }).Wait();
            bworker.RunWorkerCompleted += Bworker_RunWorkerCompleted;
        }

        private void Bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pb.Value = 100;
            lblProgress.Text = "";
            lblProgress.Visible = false;
            pb.Visible = false;

            bworker = new BackgroundWorker();
            bworker.WorkerReportsProgress = true;
        }

        private void Bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            bworker.ReportProgress(10);
            System.Threading.Thread.Sleep(300);
            bworker.ReportProgress(30);
            System.Threading.Thread.Sleep(300);
            bworker.ReportProgress(60);
            Action fobj = (Action)e.Argument;
            fobj.Invoke();
            bworker.ReportProgress(100);
        }

        

        

        private void btnAddSector_Click(object sender, EventArgs e)
        {
            OnAdd();
            


        }

        private void btnEditSector_Click(object sender, EventArgs e)
        {
            OnEdit();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            gbxItem.Visible = true;
            gbxItem.Enabled = false;
            gbxItem.Text = "";
            State = CurrentFormState.READONLY;
            pnlButtons.Enabled = true;
            bsMain.CancelEdit();
        }

        

        private void bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            btnSave.Enabled = e.ProgressPercentage == 100;
            btnCancel.Enabled = e.ProgressPercentage == 100;
            pnlButtons.Enabled = e.ProgressPercentage == 100;
            pb.Visible = e.ProgressPercentage != 100;
            lblProgress.Visible = true;
            lblProgress.Text = $"{e.ProgressPercentage}%";
            pb.Value = e.ProgressPercentage;
        }


        private void bsSectors_CurrentChanged(object sender, EventArgs e)
        {
            if (!_addinNew)
            {
                OnCurrentChange();
            }
            _addinNew = false;
        }

        public void DoWork()
        {
            Loading = true;
            if (State == CurrentFormState.NEW)
            {
                New();
                while (Loading)
                {
                    Thread.Sleep(300);
                }

            }
            else if (State == CurrentFormState.EDIT)
            {

                Edit(bsMain.Current);
                
                while (Loading)
                {
                    Thread.Sleep(300);
                }
            }
            else
            {
                MessageBox.Show("Operación no permitida");
                return;
            }
            Loading = false;
        }


       
        public string GetEntityName() => Cloud.GetCosmosEntityName<Product>();

        public string FriendlyName() => "Producto";

        

        public void Edit(object obj)
        {
            var current = (ProductInput)obj;

            current.Doses = ((IEnumerable<DosesInput>)dosesBindingSource.DataSource).ToArray();
            Cloud.PushElement(current, entityName).Wait();

        }

        public void New()
        {
            
            var current = (ProductInput)bsMain.Current;
            current.Doses = ((IEnumerable<DosesInput>)dosesBindingSource.DataSource).ToArray();


            Cloud.PushElement(current, entityName).Wait();
         
        }



        public object GetList() {
            var products = Cloud.GetElements<Product>(EntityRelated.PRODUCT);
            var mapper = config.CreateMapper();
            return products.Select(mapper.Map<ProductInput>);
        }
        public string Description() => new MdmDocs().GetInfoFromEntity((int)EntityRelated.PRODUCT).Description;
        private void gbxItem_Enter(object sender, EventArgs e)
        {

        }

        private void btnAddDoses_Click(object sender, EventArgs e)
        {
            var frm = new mantenedores.doses.Frm(null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var lst = CurrentProduct.Doses.ToList();
                lst.Add(frm.DosesInput);
                CurrentProduct.Doses = lst.ToArray();
            }
            dosesBindingSource.DataSource = CurrentProduct.Doses;
            
           
        }

        private void gbxItem_Validating(object sender, CancelEventArgs e)
        {
            
        }

        public void ChangedList(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Valida()
        {
            var current = (BaseModel)bsMain.Current as BaseModel;
            if (!string.IsNullOrWhiteSpace(current.Error))
            {   
                MessageBox.Show($"existen los siguientes errores : {current.Error}");
                return false;
            }
            return true;
        }

        private ProductInput CurrentProduct => (ProductInput)bsMain.Current;

        private DosesInput CurrentDoses => (DosesInput)dosesBindingSource.Current;

        private void bsMain_AddingNew(object sender, AddingNewEventArgs e)
        {
            _addinNew = true;
        }

        private void tbxSizeContainer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Validaciones.ValidaDecimalParaKeyPress(((TextBox)sender).Text, e.KeyChar, 10, 2);
        }

        private void btnEditDoses_Click(object sender, EventArgs e)
        {
            if (CurrentDoses !=null)
            {
                var frm = new mantenedores.doses.Frm(CurrentDoses);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var lst = CurrentProduct.Doses.ToList();
                    lst.Remove(CurrentDoses);
                    lst.Add(frm.DosesInput);
                    CurrentProduct.Doses = lst.ToArray();
                }
                dosesBindingSource.DataSource = CurrentProduct.Doses;
            }
        }
    }
}