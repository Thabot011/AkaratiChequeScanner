using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AkaratiCheckScanner;
using Newtonsoft.Json;
using ScanCRNet;
using ScanCRNet.Utility;
using Image = System.Drawing.Image;

namespace SimpleScan
{
    public partial class MainForm : Form
    {
        ScanPage sp = new ScanPage();
        static ScanCRNet.Base.LPFNSCANCALLBACK fnDecidePocket;
        private bool bScanning;
        private const string NumberTextBox = "Number";
        private const string DateTextBox = "Date";
        private const string AmountTextBox = "Amount";


        public MainForm()
        {
            InitializeComponent();

            bProbed.RegistControl(btnConfirm);
            bProbed.RegistControl(btnScanAll);
            bProbed.RegistControl(btnSetting);
            bProbed.SetProbed(false);

            bScanning = false;
            fnDecidePocket = DecidePocket;
            sp.OnPageComp += OnPage;
            sp.OnePageComp += OnItem;
            sp.ScanComp += OnScanDone;
        }


        protected override void OnLoad(EventArgs e)
        {
            string imageFolder = @"./Output"; // Update this to your folder path
            string[] imageFiles = Directory.GetFiles(imageFolder, "*.jpeg"); // Or *.png, etc.
            foreach (string file in imageFiles)
            {
                Images.Add(Image.FromFile(file));
            }

            LoadImagesWithPagination();
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            Terminate();

            base.OnClosed(e);
        }

        private class ProbeStatus
        {
            private bool bProbed = false;
            private List<Control> listRegistered = new List<Control>();

            public static bool operator true(ProbeStatus sts)
            {
                return sts.bProbed == true;
            }

            public static bool operator false(ProbeStatus sts)
            {
                return sts.bProbed == false;
            }

            public static bool operator !(ProbeStatus sts)
            {
                return sts.bProbed == false;
            }

            public bool SetProbed(bool b)
            {
                bProbed = b;

                foreach (Control ctrl in listRegistered)
                {
                    ctrl.Enabled = b;
                }

                return bProbed;
            }

            public void RegistControl(Control ctrl)
            {
                listRegistered.Add(ctrl);
            }
        }
        ProbeStatus bProbed = new ProbeStatus();

        /// <summary>
        /// Probe
        /// Initialize driver and set the default value.
        /// </summary>
        private void Probe()
        {
            if (bProbed) return;

            try
            {
                CheckStatus(Csd.ProbeEx());

                bProbed.SetProbed(true);

                // Typical Settings
                Csd.ParSet(CSDP.FEEDER, Csd.FEEDER.SIMPLEX);
                Csd.ParSet(CSDP.MICR, true);
                Csd.ParSet(CSDP.MICR_REMOVE_SPACE, false);
                Csd.ParSet(CSDP.AUTOSIZE, true);
                Csd.ParSet(CSDP.MODE, Csd.MODE.GRAYSCALE_SMOOTHING);
                Csd.ParSet(CSDP.SORTBY, Csd.SORTBY.PC);
                Csd.ParSet(CSDP.CALLBACK_FUNC, fnDecidePocket);

            }
            catch (Exception)
            {

                bProbed.SetProbed(false);
            }
        }

        private void Terminate()
        {
            if (!bProbed) return;

            Csd.Terminate();
            bProbed.SetProbed(false);
        }

        private void btnProbe_Click(object sender, EventArgs e)
        {
            Probe();
        }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            Terminate();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            DriverDialog.Show(this);
        }


        /// <summary>
        /// Check the status.
        /// </summary>
        /// <param name="nStatus"></param>
        /// <param name="ignore"></param>
        private void CheckStatus(int nStatus)
        {
            if (nStatus == Csd.CODE.OK) return;

            string msg = string.Empty;
            if (bProbed)
            {
                msg = ScanUtility.GetErrorMsg(nStatus);
            }
            else
            {
                msg = "ErrorCode : " + nStatus.ToString();
            }
            MessageBox.Show(msg);
            throw new Exception(msg);
        }

        private bool IsDuplex()
        {
            int n = 0;
            CheckStatus(Csd.ParGet(CSDP.FEEDER, ref n));

            if (n == Csd.FEEDER.DUPLEX)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Callback from the driver
        ///   dwReason == Csd.ReasonOfCallBack.MICR, choose output pocket
        /// </summary>
        /// <param name="dwReason"></param>
        /// <param name="lParam"></param>
        /// <param name="nStatus"></param>
        public void DecidePocket(int dwReason, int lParam, int nStatus)
        {
            if (dwReason == Csd.ReasonOfCallBack.MICR)
            {
                StringBuilder micr = new StringBuilder(70);
                int Status = Csd.ParGet(CSDP.MICRDATA, micr);
                if (nStatus == Csd.CODE.OK)
                {
                    string s = micr.ToString();

                    // If MICR contains '@' (Miss-read), then reject to 3.
                    // If MICR is empty, then output is 2.
                    // Good MICR string, then output is 1.
                    if (s.Contains("@"))
                    {
                        Csd.ParSet(CSDP.DECIDEPOCKET, Csd.SORTPOCKET.NO3);
                    }
                    else if (s.Length == 0)
                    {
                        Csd.ParSet(CSDP.DECIDEPOCKET, Csd.SORTPOCKET.NO2);
                    }
                    else
                    {
                        Csd.ParSet(CSDP.DECIDEPOCKET, Csd.SORTPOCKET.NO1);
                    }
                }
            }
        }

        void OnPage(object sender, OnPageEventArgs e)
        {
            if (e is OnPageEventArgs)
            {
                ScannedItemPair pair = ((OnPageEventArgs)e).Pair;

                // if you want to get information on each side, 
                // you can get them here.

            }
        }

        void OnItem(object sender, OnePageCompletedEventArgs e)
        {
            if (e is OnePageCompletedEventArgs)
            {
                ScannedItemPair pair = ((OnePageCompletedEventArgs)e).Pair;
                //picFront.Image = pair.ItemFront.ImgResult;
                this.Images.Add(pair.ItemFront.ImgResult);
                // MICR
                // textMICR.Text = pair.MicrString;
            }
        }

        private List<Image> Images = new List<Image>();

        private void CreateStackedPictureBoxes(List<Image> images)
        {
            pnlContainer.Controls.Clear();
            pnlContainer.AutoScroll = true;
            pnlContainer.SuspendLayout();
            int yPos = 0;
            int totalHeight = 0;
            foreach (Image img in images)
            {
                PictureBox pb = new PictureBox();
                pb.Image = img;
                pb.Width = pnlContainer.ClientSize.Width - 20; // Account for scrollbar
                pb.Height = img.Height * pb.Width / img.Width; // Maintain aspect ratio
                pb.Location = new Point(0, yPos);
                pb.Size = new System.Drawing.Size(566, 200);
                pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                pb.BackColor = System.Drawing.SystemColors.AppWorkspace;
                pb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

                pnlContainer.Controls.Add(pb);
                yPos += pb.Height + 5; // Add small gap between images
                pnlContainer.ScrollControlIntoView(pb);
                totalHeight += pb.Height;
            }
            pnlContainer.ResumeLayout();
            pnlContainer.ScrollControlIntoView(pnlContainer.Controls[pnlContainer.Controls.Count - 1]);
            pnlContainer.Height = totalHeight + 200;
        }

        private void CreateBalancedTwoColumnPictureBox(List<Image> images)
        {
            pnlContainer.Controls.Clear();
            pnlContainer.AutoScroll = true;

            if (images == null || images.Count == 0) return;

            int columnWidth = (pnlContainer.ClientSize.Width - 30) / 2;
            int padding = 10;
            int totalHeight = 0;
            // Split images into two lists
            var leftImages = new List<Image>();
            var rightImages = new List<Image>();

            for (int i = 0; i < images.Count; i++)
            {
                if (i % 2 == 0) leftImages.Add(images[i]);
                else rightImages.Add(images[i]);
            }

            // Draw left column
            int yPos = padding;
            foreach (var img in leftImages)
            {
                int height = (int)(columnWidth * ((float)img.Height / img.Width));
                totalHeight += height;
                var pb = new PictureBox
                {
                    Image = img,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = columnWidth,
                    Height = height,
                    BackColor = System.Drawing.SystemColors.AppWorkspace,
                    Location = new Point(5, yPos),
                    BorderStyle = BorderStyle.Fixed3D
                };
                pnlContainer.Controls.Add(pb);
                yPos += height + padding;
            }

            // Draw right column
            yPos = padding;
            foreach (var img in rightImages)
            {
                int height = (int)(columnWidth * ((float)img.Height / img.Width));
                var pb = new PictureBox
                {
                    Image = img,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = columnWidth,
                    Height = height,
                    Location = new Point(columnWidth + 20, yPos),
                    BorderStyle = BorderStyle.FixedSingle
                };
                pnlContainer.Controls.Add(pb);
                yPos += height + padding;
            }

            // Add bottom padding
            pnlContainer.Controls.Add(new Panel { Height = padding, Width = 1 });
            pnlContainer.Height = totalHeight + 60;
        }

        /// <summary>
        /// ScanComplete Event occur.
        /// </summary>
        void OnScanDone(object sender, ScanCompletedEventArgs e)
        {
            string strMsg = string.Empty;

            if (e.Error != null)
            {
                strMsg = e.Error.Message;
                MessageBox.Show(strMsg);
            }
            else
            {
                //CreateStackedPictureBoxes(this.Images);
                //CreateBalancedTwoColumnPictureBox(Images);
                LoadImagesWithPagination();

            }

            bScanning = false;
        }



        private void btnScanPage_Click(object sender, EventArgs e)
        {
            if (bScanning) return;

            // Scan A Page.
            try
            {
                CheckStatus(Csd.ParSet(CSDP.MAXDOCUMENT, 1));

                bScanning = true;
                Guid taskid = Guid.NewGuid();
                sp.DoScanAsync(taskid);
            }
            catch (Exception)
            {
            }
        }
        private int currentPage = 0;
        private const int ItemsPerPage = 4;
        public void LoadImagesWithPagination()
        {

            currentPage = 0;
            UpdatePaginationButtons();
            ShowCurrentPage();
        }

        private void UpdatePaginationButtons()
        {
            if (Images == null || Images.Count == 0)
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                return;
            }

            btnPrevious.Enabled = currentPage > 0;
            btnNext.Enabled = (currentPage + 1) * ItemsPerPage < Images.Count;
        }
        private void Reset()
        {
            this.pnlContainer.Controls.Clear();
            this.Images = new List<Image>();
        }
        private void btnScanAll_Click(object sender, EventArgs e)
        {
            if (bScanning) return;

            // Scan all pages of the feeder.
            try
            {
                this.Reset();
                CheckStatus(Csd.ParSet(CSDP.MAXDOCUMENT, 0));

                bScanning = true;
                Guid taskid = Guid.NewGuid();
                sp.DoScanAsync(taskid);
            }
            catch (Exception)
            {
            }
        }
        private void ShowCurrentPage()
        {
            pnlContainer.Controls.Clear();

            if (Images == null || Images.Count == 0) return;

            int startIndex = currentPage * ItemsPerPage;
            int endIndex = Math.Min(startIndex + ItemsPerPage, Images.Count);
            int itemsToShow = endIndex - startIndex;

            if (itemsToShow <= 0) return;

            var pageImages = new Bitmap[itemsToShow];
            Array.Copy(Images.ToArray(), startIndex, pageImages, 0, itemsToShow);

            DisplayTwoColumnImages(pageImages);
        }

        private void DisplayTwoColumnImages(Bitmap[] images)
        {
            int columnWidth = 400;
            int padding = 10;
            int xLeft = 5;
            int xRight = columnWidth + 20;
            int yLeft = padding;
            int yRight = padding;

            FlowLayoutPanel imagesLayoutPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Top
            };
            for (int i = 0; i < images.Length; i++)
            {
                Bitmap img = images[i];
                bool isLeftColumn = i % 2 == 0;

                float ratio = (float)img.Height / img.Width;
                int displayHeight = (int)(columnWidth * ratio);

                var pb = new PictureBox
                {
                    Image = img,
                    Name = i.ToString(),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = columnWidth,
                    Height = displayHeight,
                    BackColor = System.Drawing.SystemColors.AppWorkspace,
                    BorderStyle = BorderStyle.Fixed3D,
                };



                GroupBox groupBox = new GroupBox()
                {
                    Name = pb.Name,
                    AutoSize = true,
                };


                groupBox.Controls.Add(pb);

                //TODO: Addd Textboxes here
                AddTxtBoxesUnderPictureBox(pb, groupBox);

                imagesLayoutPanel.Controls.Add(groupBox);

                if (isLeftColumn)
                    yLeft += displayHeight + padding;
                else
                    yRight += displayHeight + padding;
            }

            pnlContainer.Controls.Add(imagesLayoutPanel);

            // Add bottom padding
            pnlContainer.Controls.Add(new Panel { Height = padding });
        }

        private void AddTxtBoxesUnderPictureBox(PictureBox pb, GroupBox groupBox)
        {
            int textBoxWidth = 130;
            int spacing = 20;

            TextBox NumbertxtBox = new TextBox();
            NumbertxtBox.Name = "Number";
            NumbertxtBox.Width = textBoxWidth;
            NumbertxtBox.Left = pb.Left;
            NumbertxtBox.Top = pb.Bottom + spacing;
            AddPlaceholder(NumbertxtBox, "Number");

            TextBox DatetxtBox = new TextBox();
            DatetxtBox.Name = "Date";
            DatetxtBox.Width = textBoxWidth;
            DatetxtBox.Left = pb.Left + 1 * (textBoxWidth + spacing);
            DatetxtBox.Top = pb.Bottom + spacing;
            AddPlaceholder(DatetxtBox, "Date");


            TextBox AmounttxtBox = new TextBox();
            AmounttxtBox.Name = "Amount";
            AmounttxtBox.Width = textBoxWidth;
            AmounttxtBox.Left = pb.Left + 2 * (textBoxWidth + spacing);
            AmounttxtBox.Top = pb.Bottom + spacing;
            AddPlaceholder(AmounttxtBox, "Amount");

            groupBox.Controls.AddRange(new Control[] { NumbertxtBox, DatetxtBox, AmounttxtBox });
        }

        private void AddPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowCurrentPage();
                UpdatePaginationButtons();
                pnlContainer.VerticalScroll.Value = 0;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((currentPage + 1) * ItemsPerPage < Images.Count)
            {
                currentPage++;
                ShowCurrentPage();
                UpdatePaginationButtons();
                pnlContainer.VerticalScroll.Value = 0;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            foreach (Image image in Images)
            {
                var fileName = $"{Guid.NewGuid().ToString()}.jpeg";
                if (!Directory.Exists("Output"))
                {
                    Directory.CreateDirectory("Output");
                }
                image.Save(Path.Combine("Output", fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            GetChequeModel();
        }



        private object GetChequeModel()
        {
            CreateChequesRequestDto createChequesRequestDto = new CreateChequesRequestDto();

            createChequesRequestDto.CustomerParticipantId = comboBox1.SelectedIndex;

            foreach (var layoutpanel in pnlContainer.Controls)
            {

                if (layoutpanel is FlowLayoutPanel)
                {
                    var lPanel = layoutpanel as FlowLayoutPanel;
                    foreach (var gb in lPanel.Controls)
                    {
                        if (gb is GroupBox)
                        {
                            var lGroup = gb as GroupBox;
                            foreach (var control in lGroup.Controls)
                            {
                                if (control is PictureBox)
                                {

                                }
                                if (control is TextBox)
                                {
                                    var c = control as TextBox;
                                    switch (c.Name)
                                    {
                                        case NumberTextBox:
                                            {
                                                break;
                                            }

                                        case DateTextBox:
                                            {
                                                break;
                                            }
                                        case AmountTextBox:
                                            {
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return createChequesRequestDto;
        }

        private void CallAddChequeAPI()
        {

        }

        private async void comboBox2_TextChanged(object sender, EventArgs e)
        {
            string userInput = comboBox2.Text;
            if (userInput.Length > 2)
            {
                var suggestions = await GetCustomersApiCallAsync(userInput);
                comboBox2.DisplayMember = "Name";
                comboBox2.ValueMember = "Id";
                comboBox2.Items.AddRange(suggestions.ToArray());
            }
        }

        private async Task<List<LookupItem>> GetCustomersApiCallAsync(string searchTerm)
        {
            try
            {
                // Replace with your actual API endpoint
                // var baseUrl = ConfigurationManager.AppSettings["ApiUrl"];
                var baseUrl = "http://localhost:5049/v1";
                baseUrl = baseUrl + "/lookups/customers";
                string apiUrl = $"{baseUrl}/{searchTerm}";
                
                // Send the API request and get the response
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GlobalSetting.AuthToken}");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();  // Throws an exception if the status code is not successful

                    // Read the API response
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the API response (Assuming JSON in this case)
                    var suggestions = JsonConvert.DeserializeObject<List<LookupItem>>(responseContent);
                    return suggestions;

                }

            }

            catch (Exception ex)
            {
                // Handle API errors (e.g., network issues, invalid response, etc.)
                MessageBox.Show($"Error fetching suggestions: {ex.Message}");
                return null;
            }
        }
    }

    public class LookupItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
    };

}
