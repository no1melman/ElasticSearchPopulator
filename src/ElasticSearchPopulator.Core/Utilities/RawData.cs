namespace ElasticSearchPopulator.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed class RawData
    {
        private string RawNames = @"Irina Sutphin  
Gordon Santangelo  
Fidel Weedon  
Korey Dyches  
Angelique Goodsell  
Fanny Zoll  
Keren Beran  
Abe Bier  
Shemika Ostlund  
Jacquelin Burlingame  
Penelope Danley  
Jamaal Lechler  
Jannie Fritsche  
Roselyn Schiefelbein  
Hertha Leiser  
Sari Merwin  
Desire Catlin  
Arlena Monzo  
Juan Lagunas  
Mitchell Reyna  
Maryland Carbo  
Hershel Behrendt  
Winfred Stallings  
Joyce Steves  
Meda Bussell  
Theresia Ballweg  
Vicki Rehberg  
Yaeko Gidley  
Awilda Shorter  
Kiara Lehto  
Librada Delano  
Celestine Lasko  
Amy Mackiewicz  
Izola Nolen  
Willian Lubinski  
Kiley Tallarico  
Estela Perreault  
Candance Haefner  
Veda Kmetz  
Carolee Grimmer  
Birdie Chevalier  
Danette Emig  
Yoshiko Kemplin  
Lorelei Clerk  
Kaitlyn Demar  
Chuck Smelser  
Ligia Buhr  
My Glasper  
Denna Bohling  
Marivel Yuan";

        public List<string> FirstNames()
        {
            var firstAndLast = this.RawNames.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return firstAndLast.Select(x => x.Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim()).ToList();
        }

        public List<string> LastNames()
        {
            var firstAndLast = this.RawNames.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return firstAndLast.Select(x => x.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim()).ToList();
        }

        private string RawPlaceNames = @"Spruce Street
Church Road
Main Street
Glenwood Avenue
Adams Avenue
College Street
Williams Street
Smith Street
Clark Street
Union Street
Holly Drive
Railroad Avenue
Crescent Street
North Street
Creekside Drive
White Street
8th Street West
Penn Street
Cobblestone Court
Fairview Avenue
Vine Street
East Street
4th Street North
Hillside Drive
Maple Street
Willow Drive
B Street
Myrtle Street
Summer Street
Myrtle Avenue
Liberty Street
Heather Lane
Bridge Street
Oak Lane
Mulberry Lane
Cottage Street
Ridge Road
Green Street
Jefferson Street
8th Avenue
Garfield Avenue
Laurel Drive
Evergreen Lane
Canterbury Court
Lakeview Drive
Pheasant Run
Summit Street
Route 32
Amherst Street
Chapel Street";

        public List<string> PlaceNames()
        {
            return
                this.RawPlaceNames.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        private string RawCounties = @"AB	Aberdeen	
AL	St Albans	
B	Birmingham	
BA	Bath	
BB	Blackburn	
BD	Bradford	
BH	Bournemouth	
BL	Bolton	
BN	Brighton	
BR	Bromley	
BS	Bristol	
BT	Belfast	
CA	Carlisle	
CB	Cambridge	
CF	Cardiff	
CH	Chester	
CM	Chelmsford	
CO	Colchester	
CR	Croydon	
CT	Canterbury	
CV	Coventry	
CW	Crewe	
DA	Dartford	
DD	Dundee	
DE	Derby	
DG	Dumfries
DH	Durham	
DL	Darlington	
DN	Doncaster	
DT	Dorchester	
DY	Dudley	
E	East London	
EC	East Central London	
EH	Edinburgh	
EN	Enfield	
EX	Exeter	
FK	Falkirk	
FY	Blackpool
G	Glasgow	
GL	Gloucester	
GU	Guildford	
HA	Harrow	
HD	Huddersfield	
HG	Harrogate	
HP	Hemel Hempstead	
HR	Hereford	
HS	Outer Hebrides
HU	Hull	
HX	Halifax	
IG	Ilford
IP	Ipswich	
IV	Inverness	
KA	Kilmarnock	
KT	Kingston upon Thames	
KW	Kirkwall	
KY	Kirkcaldy	
L	Liverpool	
LA	Lancaster	
LD	Llandrindod Wells
LE	Leicester	
LL	Llandudno	
LN	Lincoln	
LS	Leeds	
LU	Luton	
M	Manchester	
ME	Rochester
MK	Milton Keynes	
ML	Motherwell	
N	North London	
NE	Newcastle upon Tyne	
NG	Nottingham	
NN	Northampton	
NP	Newport	
NR	Norwich	
NW	North West London	
OL	Oldham	
OX	Oxford	
PA	Paisley	
PE	Peterborough	
PH	Perth	
PL	Plymouth	
PO	Portsmouth	
PR	Preston	
RG	Reading	
RH	Redhill	
RM	Romford	
S	Sheffield	
SA	Swansea	
SE	South East London	
SG	Stevenage	
SK	Stockport	
SL	Slough	
SM	Sutton
SN	Swindon	
SO	Southampton	
SP	Salisbury
SR	Sunderland	
SS	Southend-on-Sea	
ST	Stoke-on-Trent	
SW	South West London	
SY	Shrewsbury	
TA	Taunton	
TD	Galashiels
TF	Telford	
TN	Tunbridge Wells
TQ	Torquay	
TR	Truro	
TS	Cleveland
TW	Twickenham	
UB	Southall
W	West London	
WA	Warrington	
WC	Western Central London	
WD	Watford	
WF	Wakefield	
WN	Wigan	
WR	Worcester	
WS	Walsall	
WV	Wolverhampton	
YO	York	
ZE	Lerwick";

        public List<GreatBritishCounty> CountiesWithPostCode()
        {
            var regex = new Regex(@"\s");

            return this.RawCounties.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
                {
                    var split = regex.Split(x);
                    return new GreatBritishCounty(split[1], split[0]);
                }).ToList();
        }
    }
}