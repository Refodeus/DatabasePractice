﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingOfDatabaseProject
{
    public class DockerfileFlyoutMenuItem
    {
        public DockerfileFlyoutMenuItem()
        {
            TargetType = typeof(DockerfileFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}