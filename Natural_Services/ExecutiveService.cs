﻿using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class ExecutiveService : IExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExecutiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive, string areaId, string cityId, string stateId)
        {
            {
                var response = new ExecutiveResponse();

                try
                {
                    // setting associated models (or) entities 

                    executive.AreaNavigation = await _unitOfWork.AreaRepo.GetByIdAsync(areaId);
                    executive.CityNavigation = await _unitOfWork.CityRepo.GetByIdAsync(cityId);
                    executive.StateNavigation = await _unitOfWork.StateRepo.GetByIdAsync(stateId);

                    // Adding distributor to the repository

                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);

                    // Commit changes
                    var created = await _unitOfWork.CommitAsync();

                    if (created != null)
                    {
                        response.Message = "Insertion Successful";
                        response.StatusCode = 200;
                    }
                }
                catch (Exception ex)
                {

                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;
                }

                return response;
            }
        }

        public  async Task<IEnumerable<Executive>> GetAllExecutives()
        {
            var result = await _unitOfWork.ExecutiveRepo.GetAllExectivesAsync();
            return result;
        }

        public  async Task<Executive> GetExecutiveById(string ExecutiveId)
        {
            return await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(ExecutiveId);
        }
    }
}
