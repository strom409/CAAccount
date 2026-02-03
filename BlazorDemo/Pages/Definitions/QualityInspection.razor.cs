@* using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

public partial class QualityInspection : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    protected QualityInspectionModel QualityModel { get; set; } = new();
    protected bool ShowImageModal { get; set; }
    protected List<string> ImagesToShow { get; set; } = new();
    protected bool IsUploading1 { get; set; }
    protected bool IsUploadDone1 { get; set; }
    protected bool SelectAllDefects { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Id > 0)
        {
            // Load existing inspection data
            QualityModel = await LoadInspectionData(Id);
        }
        else
        {
            // Initialize new inspection
            QualityModel = new QualityInspectionModel();
        }
    }

    private async Task<QualityInspectionModel> LoadInspectionData(int id)
    {
        // TODO: Implement data loading from API
        return new QualityInspectionModel
        {
            PreInwardCode = "153451",
            LotNo = "A4-8706",
            BrandName = "Gala Apple",
            Pressure1 = "13",
            Pressure2 = "14",
            Pressure3 = "15",
            AvgPressure = "14",
            AvgWeight1 = "18.1",
            AvgWeight2 = "17.3",
            AvgWeight3 = "17",
            AvgWeight = "17.47",
            BGradeAvg1 = "2.1",
            BGradeAvg2 = "3.6",
            BGradeAvgValue = "2.85",
            LessColorPercentage = "25",
            AvgSize = "E",
            HasWaterCore = true,
            HasRusting = false,
            HasTouch = true,
            HasScab = false,
            HasBitterPit = true,
            HasSunburn = false,
            HasBitterRot = false,
            HasBlouch = false,
            HasInsectHole = true,
            HasOlla = false,
            HasSanjose = true,
            HasScar = false,
            HasFlySpeck = false
        };
    }

    protected void CalculateAvgPressure()
    {
        var divideNo = 0;
        var total = 0.0;

        if (double.TryParse(QualityModel.Pressure1, out var p1))
        {
            total += p1;
            divideNo++;
        }

        if (double.TryParse(QualityModel.Pressure2, out var p2))
        {
            total += p2;
            divideNo++;
        }

        if (double.TryParse(QualityModel.Pressure3, out var p3))
        {
            total += p3;
            divideNo++;
        }

        QualityModel.AvgPressure = divideNo > 0 ? (total / divideNo).ToString("0.##") : "";
        StateHasChanged();
    }

    protected void CalculateAvgWeight()
    {
        // Similar implementation to CalculateAvgPressure
    }

    protected void CalculateBGradeAvg()
    {
        // Similar implementation to CalculateAvgPressure
    }

    protected void ToggleAllDefects()
    {
        QualityModel.HasWaterCore = SelectAllDefects;
        QualityModel.HasRusting = SelectAllDefects;
        QualityModel.HasTouch = SelectAllDefects;
        // Set all other defect properties
    }

    protected async Task HandleImageUpload(InputFileChangeEventArgs e, int imageNumber)
    {
        // TODO: Implement image upload logic
        IsUploading1 = true;
        StateHasChanged();

        // Simulate upload delay
        await Task.Delay(1000);

        IsUploading1 = false;
        IsUploadDone1 = true;

        // Save the file and get path
        QualityModel.Image1Path = $"uploaded_{imageNumber}.jpg";
        StateHasChanged();
    }

    protected void ShowImageSlider(int activeImage)
    {
        ImagesToShow = new List<string>
        {
            QualityModel.Image1Path,
            QualityModel.Image2Path,
            QualityModel.Image3Path
        }.Where(img => !string.IsNullOrEmpty(img)).ToList();

        ShowImageModal = true;
        StateHasChanged();
    }

    protected void CloseImageModal()
    {
        ShowImageModal = false;
    }

    protected string GetImagePath(string imageName)
    {
        return $"/img/QualityInspection/{imageName}";
    }

    protected async Task HandleValidSubmit()
    {
        // TODO: Implement save logic
        await SaveInspectionData(QualityModel);
        NavigationManager.NavigateTo("/qualityinspections");
    }

    private async Task SaveInspectionData(QualityInspectionModel model)
    {
        // TODO: Call API to save data
        await Task.Delay(500); // Simulate API call
    }
}

public class QualityInspectionModel
{
    public string PreInwardCode { get; set; }
    public string LotNo { get; set; }
    public string PartyGroup { get; set; }
    public string PartyName { get; set; }
    public string ItemName { get; set; }
    public string BrandName { get; set; }
    public DateTime PreInwardDate { get; set; } = DateTime.Now;
    public bool IsInvestor { get; set; }
    public string InvestorText { get; set; }

    // Images
    public string Image1Path { get; set; }
    public string Image2Path { get; set; }
    public string Image3Path { get; set; }

    // Pressure
    public string Pressure1 { get; set; }
    public string Pressure2 { get; set; }
    public string Pressure3 { get; set; }
    public string AvgPressure { get; set; }

    // Weight
    public string AvgWeight1 { get; set; }
    public string AvgWeight2 { get; set; }
    public string AvgWeight3 { get; set; }
    public string AvgWeight { get; set; }

    // B-Grade
    public string BGradeAvgType { get; set; }
    public string BGradeAvg1 { get; set; }
    public string BGradeAvg2 { get; set; }
    public string BGradeAvgValue { get; set; }

    // Other fields
    public string LessColorPercentage { get; set; }
    public string AvgSize { get; set; }
    public string Remarks { get; set; }

    // Defects
    public bool HasWaterCore { get; set; }
    public bool HasRusting { get; set; }
    public bool HasTouch { get; set; }
    public bool HasScab { get; set; }
    public bool HasBitterPit { get; set; }
    public bool HasSunburn { get; set; }
    public bool HasBitterRot { get; set; }
    public bool HasBlouch { get; set; }
    public bool HasInsectHole { get; set; }
    public bool HasOlla { get; set; }
    public bool HasSanjose { get; set; }
    public bool HasScar { get; set; }
    public bool HasFlySpeck { get; set; }
} *@