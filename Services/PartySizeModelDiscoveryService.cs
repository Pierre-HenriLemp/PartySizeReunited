using HarmonyLib;
using StoryMode.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem.GameComponents;

namespace PartySizeReunited.Services
{
    public class PartySizeModelDiscoveryService
    {
        public List<DefaultPartySizeLimitModel> DiscoverPartySizeModels()
        {
            List<DefaultPartySizeLimitModel> models = new List<DefaultPartySizeLimitModel>();
            
            try
            {
                var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic) // Évite les erreurs sur les assemblages dynamiques
                    .SelectMany(a => {
                        try {
                            return a.GetTypes();
                        } catch {
                            return Type.EmptyTypes;
                        }
                    })
                    .Where(t =>
                        t.IsSubclassOf(typeof(DefaultPartySizeLimitModel))
                        && !t.IsAbstract
                        && t.GetConstructor(Type.EmptyTypes) != null // Vérifie qu'il y a un constructeur sans paramètres
                        && t != typeof(PartySize)
                        && !t.IsAssignableFrom(typeof(StoryModePartySizeLimitModel))
                    )
                    .ToList();

                foreach (var type in derivedTypes)
                {
                    try
                    {
                        var instance = (DefaultPartySizeLimitModel)Activator.CreateInstance(type);
                        models.Add(instance);
                        Console.WriteLine($"Discovered party size model: {type.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error instantiating {type.Name}: {ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error loading types: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during model discovery: {ex.Message}");
            }
            
            return models;
        }
    }
}
