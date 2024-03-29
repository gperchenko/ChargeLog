﻿using ChargeLog.DBModels;
using ChargeLog.Models;
using ChargeLog.ViewModels;

namespace ChargeLog.Pages
{
    public partial class Import
    {
        private ImportView newImport = new ImportView();
        private List<ImportListItem> imports = new List<ImportListItem>();
        private List<string> importTypes = new List<string>();
        private List<string> fileNames = new List<string>();
        private List<Car> cars = new List<Car>();
        private List<KeyValue> groups = new List<KeyValue>();

        protected override async Task OnInitializedAsync()
        {
            imports = await ImportService.GetImportsAsync();
            Config = ConfigService.GetInterfaceConfig();
            importTypes = ImportService.GetImportTypes();
        }

        private async void ProcessImport()
        {
            await ImportService.ImportFileAsync(newImport.ImportType!);
            imports = await ImportService.GetImportsAsync();
            newImport = new ImportView();
            StateHasChanged();
        }
    }
}
