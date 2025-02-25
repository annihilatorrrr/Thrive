﻿using System;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   Shows a label with a city name and size for selecting that city
/// </summary>
public class CityNameLabel : Button, IEntityNameLabel
{
    private string translationTemplate = null!;

    public CityNameLabel()
    {
        UpdateTranslationTemplate();
    }

    public event IEntityNameLabel.OnEntitySelected? OnEntitySelectedHandler;

    [JsonIgnore]
    public Control LabelControl => this;

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == NotificationTranslationChanged)
        {
            UpdateTranslationTemplate();
        }
    }

    public void UpdateFromEntity(IEntityWithNameLabel entity)
    {
        string newText;

        switch (entity)
        {
            case PlacedCity city:
                newText = translationTemplate.FormatSafe(city.CityName, city.Population);
                break;

            case PlacedPlanet planet:
                newText = translationTemplate.FormatSafe(planet.PlanetName, planet.Population);
                break;

            default:
                throw new ArgumentException("Unsupported entity type", nameof(entity));
        }

        // TODO: check if comparing against the old text is faster than always applying the new value
        Text = newText;
    }

    private void UpdateTranslationTemplate()
    {
        translationTemplate = TranslationServer.Translate("NAME_LABEL_CITY");
    }

    private void ForwardSelection()
    {
        OnEntitySelectedHandler?.Invoke();
    }
}
