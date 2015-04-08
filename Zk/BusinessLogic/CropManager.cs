﻿using System.Collections.Generic;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class CropManager
    {
        readonly Repository _repository;

        public CropManager(IZkContext db)
        {
            _repository = new Repository(db);
        }

        public IEnumerable<Crop> GetAll()
        {
            return _repository.GetAllCrops();
        }

        public Crop Get(int id)
        {
            return _repository.GetCrop(id);
        }

    }
}