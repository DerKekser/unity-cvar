using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kekser.UnityCVar
{
    public class CVarManager
    {
        private CVarTarget _target;
        private Dictionary<Type, List<object>> _classes = new Dictionary<Type, List<object>>();
        
        [CVar("tgt_gameobject", "Selects a game object to execute commands on")]
        public string TargetGameObject
        {
            get => _target.TargetName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _target.TargetName = value;
                    _target.TargetType = CVarTargetType.None;
                    return;
                }
                
                _target.TargetName = value;
                _target.TargetType = CVarTargetType.GameObjectList;
            }
        }
        
        [CVar("tgt_class", "Selects a specific class instance on the current target")]
        public string TargetClass
        {
            get => _target.TargetName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _target.TargetName = value;
                    _target.TargetType = CVarTargetType.None;
                    return;
                }
                
                _target.TargetName = value;
                _target.TargetType = CVarTargetType.ClassList;
            }
        }
        
        [CVar("tgt_clear", "Removes any active GameObject or class target")]
        public void ClearTarget()
        {
            _target = new CVarTarget();
        }
        
        [CVar("con_list", "Displays all registered cvars and commands")]
        public string ListCVars(string filter = "")
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, ICVar> cVar in CVarAttributeCache.Cache.OrderBy(c => c.Key))
            {
                if (!string.IsNullOrWhiteSpace(filter) && !cVar.Key.Contains(filter)) 
                    continue;
                
                if (string.IsNullOrWhiteSpace(cVar.Value.Description))
                    builder.AppendLine($"- {cVar.Key}");
                else
                    builder.AppendLine($"- {cVar.Key,-30} - {cVar.Value.Description}");
            }
            return builder.ToString();
        }
        
        [CVar("con_help", "Displays help for a specific cvar or command")]
        public string HelpCVar(string command)
        {
            if (!CVarAttributeCache.TryGetCVar(command, out ICVar cvar))
                return $"Command '{command}' not found.";
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command: {command}");
            builder.AppendLine($"Description: {cvar.Description}");
            builder.AppendLine($"Read: {(cvar.ReadType != null ? cvar.ReadType.Name : "None")}");
            builder.AppendLine($"Write: {(cvar.WriteTypes.Length > 0 ? string.Join(", ", cvar.WriteTypes.Select(t => t.Name)) : "None")}");
            return builder.ToString();
        }
        
        public void RegisterClass(object obj)
        {
            Type type = obj.GetType();
            if (!_classes.ContainsKey(type))
                _classes.Add(type, new List<object>());
            
            _classes[type].Add(obj);
        }
        
        public void UnregisterClass(object obj)
        {
            Type type = obj.GetType();
            if (!_classes.ContainsKey(type))
                return;
            
            _classes[type].Remove(obj);
        }
        
        public CVarManager()
        {
            _target = new CVarTarget(); 
            RegisterClass(this);
        }
        
        public CVarTarget GetTarget()
        {
            return _target;
        }
        
        public CVarResult ExecuteCommand(string command)
        {
            string[] args = Helper.SplitArguments(command);
            
            if (args.Length == 0)
                return new CVarResult(false, "No command given.");
            
            if (!CVarAttributeCache.TryGetCVar(args[0], out ICVar cvar))
                return new CVarResult(false, $"Command '{args[0]}' not found.");
            
            List<object> classes = GetClasses(cvar);
            if (classes.Count == 0)
                return new CVarResult(false, $"No instances of '{cvar.Type.Name}' found.");
            if (classes.Count > 1)
                return ListTargets(classes);
            
            return cvar.Execute(classes[0], args.Skip(1).ToArray());
        }
        
        private CVarResult ListTargets(List<object> classes)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Multiple targets found. Please specify the target.");
            for (int i = 0; i < classes.Count; i++)
            {
                object obj = classes[i];
                if (obj is UnityEngine.Object component)
                    builder.AppendLine($"{i}: {component.name,-30} -> tgt_gameobject {component.GetInstanceID()}");
                else if (obj != null && _classes.ContainsKey(obj.GetType()))
                    builder.AppendLine($"{i}: {obj.GetType().Name,-30} -> tgt_class {_classes[obj.GetType()].IndexOf(obj)}");
            }
            return new CVarResult(false, builder.ToString());
        }

        private List<object> FilterByTarget(List<object> classes)
        {
            if (_target.TargetType == CVarTargetType.None)
                return classes;
            
            if (_target.TargetType == CVarTargetType.GameObjectList)
                return classes.Where(obj => obj is UnityEngine.Object component && (component.name == _target.TargetName || component.GetInstanceID().ToString() == _target.TargetName)).ToList();
            
            if (_target.TargetType == CVarTargetType.ClassList)
                return classes.Where(obj => obj != null && _classes.ContainsKey(obj.GetType()) && _classes[obj.GetType()].IndexOf(obj).ToString() == _target.TargetName).ToList();
            
            return classes;
        }
        
        private List<object> GetClasses(ICVar cvar)
        {
            if (cvar.Type.IsStatic() || cvar.Static)
                return new List<object> { null };   
            
            List<object> classes = new List<object>();
            if (_classes.TryGetValue(cvar.Type, out List<object> classList))
                classes.AddRange(classList);
            if (cvar.Type.IsSubclassOf(typeof(UnityEngine.Object)))
                classes.AddRange(UnityEngine.Object.FindObjectsOfType(cvar.Type, true));
            classes = classes.Distinct().ToList();
            
            if (classes.Count <= 1)
                return classes;
            List<object> filteredClasses = FilterByTarget(classes.Distinct().ToList());
            if (filteredClasses.Count == 0)
                return classes;
            return filteredClasses;
        }
    }
}