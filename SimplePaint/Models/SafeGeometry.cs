﻿using Avalonia.Media;
using System;

namespace SimplePaint.Models {
    public class SafeGeometry: ForcePropertyChange, ISafe {
        private Geometry geom = Geometry.Parse("");
        private string geom_str = ""; 
        private bool valid = true;
        private readonly Action<object?>? hook;
        private readonly object? inst;
        public SafeGeometry(string init, Action<object?>? hook = null, object? inst = null) {
            this.hook = hook; this.inst = inst;
            Set(init);
            if (!valid) throw new FormatException("Неверный формат инициализации SafeGeometry: " + init);
        }
        public Geometry Geometry => geom;

        private void Upd_valid(bool v) {
            valid = v;
            hook?.Invoke(inst);
        }
        private void Re_check() {
            if (!valid) {
                valid = true;
            }
        }

        public bool Valid => valid;

        public void Set(string str) {
            Geometry data;
            try {
                data = Geometry.Parse(str);
            } catch { Upd_valid(false); return; }

            geom = data;
            geom_str = str;
            Upd_valid(true);
        }

        public string Value {
            get { Re_check(); return geom_str; }
            set {
                Set(value);
                UpdProperty(nameof(Color));
            }
        }

        public IBrush Color { get => valid ? Brushes.BlueViolet : Brushes.Red; }
    }
}
