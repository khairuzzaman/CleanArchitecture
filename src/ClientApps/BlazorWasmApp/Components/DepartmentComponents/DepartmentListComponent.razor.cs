﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorApps.Shared.Common;
using BlazorApps.Shared.Models.DepartmentModels;
using BlazorApps.Shared.Services;
using TanvirArjel.Blazor.Utilities;

namespace BlazorWasmApp.Components.DepartmentComponents
{
    public partial class DepartmentListComponent
    {
        private readonly DepartmentService _departmentService;
        private readonly ExceptionLogger _exceptionLogger;

        public DepartmentListComponent(DepartmentService departmentService, ExceptionLogger exceptionLogger)
        {
            _departmentService = departmentService;
            _exceptionLogger = exceptionLogger;
        }

        private List<DepartmentDetailsModel> Departments { get; set; }

        private string ErrorMessage { get; set; }

        private CreateDepartmentModalComponent CreateModal { get; set; }

        private UpdateDepartmentModalComponent UpdateModal { get; set; }

        private DepartmentDetailsModalComponent DetailsModal { get; set; }

        private DeleteDepartmentModalComponent DeleteModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadDepartmentsAsync();
            }
            catch (HttpRequestException httpException)
            {
                if ((int)httpException.StatusCode == 401)
                {
                    ErrorMessage = ErrorMessages.Http401ErrorMessage;
                }
                else if ((int)httpException.StatusCode == 403)
                {
                    ErrorMessage = ErrorMessages.Http403ErrorMessage;
                }
                else if ((int)httpException.StatusCode == 500)
                {
                    ErrorMessage = ErrorMessages.Http500ErrorMessage;
                }
                else
                {
                    ErrorMessage = ErrorMessages.ServerDownOrCorsErrorMessage;
                }

                await _exceptionLogger.LogAsync(httpException);
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                ErrorMessage = ErrorMessages.ClientErrorMessage;
            }
        }

        // EventHandler which will be called whenvever ItemUpdated event is published.
        private async void OnItemChanged()
        {
            await LoadDepartmentsAsync();
            StateHasChanged();
        }

        private async Task LoadDepartmentsAsync()
        {
            Departments = await _departmentService.GetListAsync();
        }

        private void ShowCreateModal()
        {
            CreateModal.Show();
        }

        private async Task ShowUpdateModal(Guid departmentId)
        {
            await UpdateModal.ShowAsync(departmentId);
        }

        private async Task ShowDetailsModalAsync(Guid departmentId)
        {
            await DetailsModal.ShowAsync(departmentId);
        }

        private async Task ShowDeleteModalAsync(Guid departmentId)
        {
            await DeleteModal.ShowAsync(departmentId);
        }
    }
}