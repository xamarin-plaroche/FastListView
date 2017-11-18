using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FLVDemo.Models
{
	public class Parameters
	{

		[JsonProperty("dataset")]
		public IList<string> Dataset { get; set; }

		[JsonProperty("timezone")]
		public string Timezone { get; set; }

		[JsonProperty("q")]
		public string Q { get; set; }

		[JsonProperty("rows")]
		public int Rows { get; set; }

		[JsonProperty("format")]
		public string Format { get; set; }

		[JsonProperty("facet")]
		public IList<string> Facet { get; set; }
	}

	public class Image
	{

		[JsonProperty("format")]
		public string Format { get; set; }

		[JsonProperty("thumbnail")]
		public bool Thumbnail { get; set; }

		[JsonProperty("height")]
		public int Height { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("filename")]
		public string Filename { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }
	}

	public class Fields
	{

		[JsonProperty("etat")]
		public string Etat { get; set; }

		[JsonProperty("commune")]
		public string Commune { get; set; }

		[JsonProperty("typologie")]
		public string Typologie { get; set; }

		[JsonProperty("code_postal")]
		public int CodePostal { get; set; }

		[JsonProperty("nom")]
		public string Nom { get; set; }

		[JsonProperty("adresse")]
		public string Adresse { get; set; }

		[JsonProperty("texte_descriptif")]
		public string TexteDescriptif { get; set; }

		[JsonProperty("type_innovation")]
		public string TypeInnovation { get; set; }

		[JsonProperty("reseau_social_youtube")]
		public string ReseauSocialYoutube { get; set; }

		[JsonProperty("image")]
		public Image Image { get; set; }

		[JsonProperty("contact_telephonique")]
		public string ContactTelephonique { get; set; }

		[JsonProperty("xy")]
		public IList<double> Xy { get; set; }

		[JsonProperty("site_internet")]
		public string SiteInternet { get; set; }

		[JsonProperty("reseau_social_twitter")]
		public string ReseauSocialTwitter { get; set; }

		[JsonProperty("contact_mail")]
		public string ContactMail { get; set; }

		[JsonProperty("reseau_social_facebook")]
		public string ReseauSocialFacebook { get; set; }
	}

	public class Geometry
	{

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("coordinates")]
		public IList<double> Coordinates { get; set; }
	}

	public class Record
	{

		[JsonProperty("datasetid")]
		public string Datasetid { get; set; }

		[JsonProperty("recordid")]
		public string Recordid { get; set; }

		[JsonProperty("fields")]
		public Fields Fields { get; set; }

		[JsonProperty("record_timestamp")]
		public DateTime RecordTimestamp { get; set; }

		[JsonProperty("geometry")]
		public Geometry Geometry { get; set; }
	}

	public class Facet
	{

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("path")]
		public string Path { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }
	}

	public class FacetGroup
	{

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("facets")]
		public IList<Facet> Facets { get; set; }
	}

	public class RootInovations
	{

		[JsonProperty("nhits")]
		public int Nhits { get; set; }

		[JsonProperty("parameters")]
		public Parameters Parameters { get; set; }

		[JsonProperty("records")]
		public IList<Record> Records { get; set; }

		[JsonProperty("facet_groups")]
		public IList<FacetGroup> FacetGroups { get; set; }
	}
}
