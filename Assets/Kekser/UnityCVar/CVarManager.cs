using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kekser.PowerCVar
{
    public class CVarManager
    {
        private CVarTarget _target;
        private Dictionary<Type, List<object>> _classes = new Dictionary<Type, List<object>>();
        
        [CVar("cvar_target", "Target for CVar commands.")]
        public string Target
        {
            get
            {
                if (_target.TargetType == CVarTargetType.None)
                    return "None";
                if (_target.TargetType == CVarTargetType.GameObjectList)
                    return $"GameObject: {_target.TargetName}";
                if (_target.TargetType == CVarTargetType.ClassList)
                    return $"Class: {_target.TargetName}";
                return "Unknown";
            }
        }
        
        [CVar("cvar_target.gameobject", "Target GameObject for CVar commands.")]
        public string TargetGameObject
        {
            get => _target.TargetName;
            set
            {
                _target.TargetName = value;
                _target.TargetType = CVarTargetType.GameObjectList;
            }
        }
        
        [CVar("cvar_target.class", "Target class for CVar commands.")]
        public string TargetClass
        {
            get => _target.TargetName;
            set
            {
                _target.TargetName = value;
                _target.TargetType = CVarTargetType.ClassList;
            }
        }
        
        [CVar("cvar_target.clear", "Clear the target for CVar commands.")]
        public void ClearTarget()
        {
            _target = new CVarTarget();
        }
        
        [CVar("cvar_list", "List all available CVars. Can be filtered by name.")]
        public string ListCVars(string filter = "")
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, CVarCache> cVar in CVarAttributeCache.Cache)
            {
                if (!string.IsNullOrWhiteSpace(filter) && !cVar.Key.Contains(filter)) 
                    continue;
                
                if (string.IsNullOrWhiteSpace(cVar.Value.Description))
                    builder.AppendLine(cVar.Key);
                else
                    builder.AppendLine($"{cVar.Key,-30} - {cVar.Value.Description}");
            }
            builder.Length -= 1;
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
            
            if (!CVarAttributeCache.TryGetCVar(args[0], out CVarCache cvar))
                return new CVarResult(false, $"Command '{args[0]}' not found.");
            
            return ExecuteCVar(cvar, args.Skip(1).ToArray());
        }
        
        private CVarResult ExecuteCVar(CVarCache cvar, string[] args)
        {
            MemberInfo member = cvar.MemberInfo;
            if (member is FieldInfo field)
                return ExecuteCVarField(cvar, args, field);
            if (member is PropertyInfo property)
                return ExecuteCVarProperty(cvar, args, property);
            if (member is MethodInfo method)
                return ExecuteCVarMethod(cvar, args, method);
            
            return new CVarResult(false, $"CVar '{cvar.Name}' Member type '{member.MemberType}' in '{cvar.Type.Name}' not supported.");
        }
        
        private CVarResult ExecuteCVarField(CVarCache cvar, string[] args, FieldInfo field)
        {
            List<object> classes = GetClasses(cvar);
            if (classes.Count == 0)
                return new CVarResult(false, $"No instances of '{cvar.Type.Name}' found.");
            if (classes.Count > 1)
                return ListTargets(classes);
            
            if (args.Length == 0)
                return new CVarResult(true, field.GetValue(classes[0]).ToString());
            
            object value = Helper.ConvertValue(args[0], field.FieldType);
            field.SetValue(classes[0], value);
            
            return new CVarResult(true, "");
        }
        
        private CVarResult ExecuteCVarProperty(CVarCache cvar, string[] args, PropertyInfo property)
        {
            List<object> classes = GetClasses(cvar);
            if (classes.Count == 0)
                return new CVarResult(false, $"No instances of '{cvar.Type.Name}' found.");
            if (classes.Count > 1)
                return ListTargets(classes);
            
            if (args.Length == 0 && property.CanRead)
                return new CVarResult(true, property.GetValue(classes[0]).ToString());
            if (args.Length == 0 && !property.CanRead)
                return new CVarResult(false, $"Property '{cvar.Name}' is write-only.");
            if (!property.CanWrite)
                return new CVarResult(false, $"Property '{cvar.Name}' is read-only.");
            
            object value = Helper.ConvertValue(args[0], property.PropertyType);
            property.SetValue(classes[0], value);
            
            return new CVarResult(true, "");
        }
        
        private CVarResult ExecuteCVarMethod(CVarCache cvar, string[] args, MethodInfo method)
        {
            List<object> classes = GetClasses(cvar);
            if (classes.Count == 0)
                return new CVarResult(false, $"No instances of '{cvar.Type.Name}' found.");
            if (classes.Count > 1)
                return ListTargets(classes);
            
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo[] optionalParameters = parameters.Where(p => p.IsOptional).ToArray();
            if (args.Length < parameters.Length - optionalParameters.Length || args.Length > parameters.Length)
                return new CVarResult(false, $"Method '{cvar.Name}' expects {parameters.Length} arguments.");
            
            object[] arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                arguments[i] = i < args.Length ? Helper.ConvertValue(args[i], parameters[i].ParameterType) : parameters[i].DefaultValue;
            
            object result = method.Invoke(classes[0], arguments);
            return new CVarResult(true, result?.ToString() ?? "");
        }

        private CVarResult ListTargets(List<object> classes)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Multiple targets found. Please specify the target.");
            for (int i = 0; i < classes.Count; i++)
            {
                object obj = classes[i];
                if (obj is UnityEngine.Component component)
                    builder.AppendLine($"{i}: {component.name} -> cvar_target.gameobject {component.GetInstanceID()}");
                else if (obj != null && _classes.ContainsKey(obj.GetType()))
                    builder.AppendLine($"{i}: {obj.GetType().Name} -> cvar_target.class {_classes[obj.GetType()].IndexOf(obj)}");
            }
            builder.Length -= 1;
            return new CVarResult(false, builder.ToString());
        }

        private List<object> FilterByTarget(List<object> classes)
        {
            if (_target.TargetType == CVarTargetType.None)
                return classes;
            
            if (_target.TargetType == CVarTargetType.GameObjectList)
                return classes.Where(obj => obj is UnityEngine.Component component && (component.name == _target.TargetName || component.GetInstanceID().ToString() == _target.TargetName)).ToList();
            
            if (_target.TargetType == CVarTargetType.ClassList)
                return classes.Where(obj => obj != null && _classes.ContainsKey(obj.GetType()) && _classes[obj.GetType()].IndexOf(obj).ToString() == _target.TargetName).ToList();
            
            return classes;
        }
        
        private List<object> GetClasses(CVarCache cvar)
        {
            if (cvar.Type.IsStatic() || cvar.MemberInfo.IsStatic())
                return new List<object> { null };   
            
            List<object> classes = new List<object>();
            if (_classes.TryGetValue(cvar.Type, out List<object> classList))
                classes.AddRange(classList);
            if (cvar.Type.IsSubclassOf(typeof(UnityEngine.Object)))
                classes.AddRange(UnityEngine.Object.FindObjectsOfType(cvar.Type));
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