﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Interfaces {
    public interface ISpotifyEntity {

        string ID { get; set; }
        string Name { get; set; }

        string GetInsertStatement();

    }
}
