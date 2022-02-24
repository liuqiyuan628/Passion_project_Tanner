# F1 Information Application
Version 2 of the passion project. Completed the CRUD operations for all the entities, including Driver, Team and Sponsor. As well as the association between sponsors and drivers. </br>

<h3> Entities Relationship </h3> 
</br>A driver can have MANY Sponsors but can only drive for ONE team. 
</br>A team can have 2 or MORE drivers.

<h3> CRUD </h3> 
</br> CRUD for driver,team and sponsor.
</br>Added an API call for listing all sponsors for a particular driver
</br>https://localhost:44359/Driver/Details/6
</br>Added an API call for listing all drivers under a particular sponsor
</br>https://localhost:44359/Sponsor/Details/6


<h3> Association </h3> 
</br> A driver can associate with one or multible sponsors, if the sponsorship end, we can unassciate the driver and the sponsor's relationship

<h3> Views </h3> 
</br> Added views to show Create, Read, Update, and Delete.
</br> Changed some style to make the web more readble.
