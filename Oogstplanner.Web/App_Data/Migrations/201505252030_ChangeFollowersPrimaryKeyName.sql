/* * * * * * * * * * * * * * * * * * * * * * *
 *
 * Migration:		ChangeFollowersPrimaryKeyName
 *
 * Date and time:	5/25/2015 8:30:21 PM
 *
 * * * * * * * * * * * * * * * * * * * * * * */

ALTER TABLE "public"."Followers" DROP CONSTRAINT "PK_public.Followers";
ALTER TABLE "public"."Followers" ADD "Id" serial NOT NULL;
ALTER TABLE "public"."Followers" ADD CONSTRAINT "PK_public.Followers" PRIMARY KEY ("Id");
ALTER TABLE "public"."Followers" DROP COLUMN "FollowerId";
insert into "public"."__MigrationHistory" ("MigrationId", "ContextKey", "Model", "ProductVersion")
 values ('201505251829178_ChangeFollowersPrimaryKeyName', 'Oogstplanner.Migrations.Configuration',  decode('1F8B0800000000000003EC5DDB6EE436127D5F60FF41D06330E9F665C6EB18DD099C9E71E26C7C81DB09F66D404B745B58DD96A23C3616FB6579C827ED2F2CA92BAF1229A9ED9E855F066E913CAC2A569155248BF3DF3FFE5CFCF01485CE23445990C44B777FB6E73A30F6123F88374B37C7F7DF1EBB3F7CFFD7BF2C3EF9D193F37B5DEF90D6232DE36CE93E609C9ECCE799F7002390CDA2C0434996DCE399974473E027F383BDBDEFE6FBFB7348205C82E5388B9B3CC641048B1FE4E72A893D98E21C8417890FC3ACFA4E4AD605AA73092298A5C0834BF72AD964380D411C4334FB0830709DD330008490350CEF5D871424186042E6C96F195C6394C49B754A3E80F0F63985A4DE3D083358917FD25637E564EF8072326F1BD6505E9EE124B204DC3FAC4433179B0F12B0DB888E08EF1311327EA65C17025CBA2B10C2D807C875C4CE4E5621A21505F996C331ABDBBD7314A5EF1AB520DA33FB707470383B3C3AFEDBD13B67958738477019C31C2310BE73AEF3BB30F0FE0E9F6F937FC27819E761C8124C482665DC07F2E91A252944F8F906DE576C9CFBAE33E7DBCDC5864D33A64DC9E0798C0F0F5CE792740EEE42D8E803238C354E10FC09121E0186FE35C018A29862C042A252EF425FF4DFBA37A280C4945CE7023CFD0AE30D7E58BAE44FD7390B9EA05F7FA928F82D0E88E5914618E550EAE4123C069B823EA1BB338022D2C7A947CB32D7B98161512D7B08D2D22A665C95CFAD1A9CA124BA494211A4A9F1F916A00DC48499A4B3DA3AC991674132B14CA424B4412C6BB4F471054D7F35597C694D344BCD62DE5A43A78D701CDA1A0AD7F8CD5A8CACE52289F143632E5409AA2F529FDD38F5883140E527FAC11A6D457EAE12B24CF509C254E55BA39BD23E4543E831636362C94F13428B6A5A2249691F81B4CA28732D49B05CCE489B37E37CC5A54CE8E40678DBEF6445F8DF24E879EB1DFD84922F04F936682567387788131A82E01AA252C34BA08F09513BD3863F828D5DBB6B1478506808BD2002A1EB5C23F25715241CBBCEDA03948F833EC875218B6242CF2698E37F06E811667838A4F1BC52FA1F76F30A6DF336AFECCEBC7246E87F918E3E452008B7DECB698E1FA874BD6204D6642072DE00980AD0AFCBADFD1E58C093C8BA911BFDBB9CCE467B3FEAF0C4D0EB178311754C601C362561987C81504D5201C9546969E24B240747281E178B9430D61351DDEE6D32524F467D3AE1AB7DDF4AAA9FDB5A8CEB2B16CA9EAF54C3D62B2F1B16934B3F7945352D7DA4B48F405A6594F65E832CFB9220FF066610171A64ABC732C29B461B2DAF2FB3205563BAED5EC8CA4316B328B55C8EF47A9A478C96565EE3797616824D569364A7A705C636549348CA87287CA6F104A33ABCBC2E60740751ED5825784D2DF67710E6E4E79E245DAEF62F20CE018D89AAEAFBDDD5CFE01DE2EACBFE3F57FF0220EFA1A9FCBEBBF2698AA8CE56958FFB9019A28F7A98CC63D8543EEC21F9973C6C918FFA48CE37444F5A420E7AA85EC31417BF5B017EE821FECAC309DBE0C37E0F0397C923DFC5FEDE410F1B24C813A8DA7B7F2C1B5369361DA6C46EB735F65459A69D41B548BB6155EB6A3D35B3AA364AD51996993855EEFC48B9CA90BB21E0D338899FA384726826638E133B319F6659E20585B4547BFEEDF62BDFFDA7D877CCF662DB884C384CB820C20B52222EB22C2DDD6F240E7BBB6822A0B68B76B39847DFE7C927E857F14718420C9D1295C64F99077C79012652F339293212B31224DDB932E490DB2B9E5E80EC3E332BBC626F8D47DD9BCD24353216801036EAA8D3C5902D656524DC475917A8825D15E8378379E5A3761D559A83BB7E05EE1A5DCDBE4037A7AF631152D0A8D5587D04C9D843B32360610ADAC0D35ADF86334E17434302B9D87452D6D990763ADECB306345620010C4ADE3C6ACAAB40C3E61C56A4DBAAE16EC3AF41059A1D8C4A397F7D1DAE8463224491E3C88786140421266DD1E383A892AE92926D79EC654F6AAC6E598F431520DAF9287465D7A40E44D06159C6A33430066D443236BE66497A9DC7D062CCE59C66E48C3A73CE0736BD0DA6A18504615C5D9929784BD940AC5E99590E45D98FA17934886F52858A994C6305A22E2FEB32C8D2E57C3C4D96088AE8CB083798D6FD187318071E12C40E6BBC3ED30703C4C14D8C0D5D802E38AED65850974BB13860E056B00ED14DAA5FB5A1762FB8228C2E85E4948FE85A987315616AC4F314218F56E65E34634658B797909B6FAB0986B6ECB2E2E409A126A98DBB3D517675D5E9D5D7DBBB6BF541A9518732F53DC2D6DA86D7AC209021B289492AE09A56701CA30BDB17B07E8CEC5CA8FA46A2AA749B37AD73D8A7E913C70F54A5EB7A07FCB3E5A719778A65B765B619E11FE2218E38255A8984EE4A60EBDC70C4280142702AB24CCA35877AAD0D5BA3CD966DB975F6484C55CA05D94D15C129214FEF052371A13616D1D3730BC4B6A3F3A3DEDB73344D5D9020B507D32C7A89D701644ED9877A130D7195920E6F3CEA84DE9418D3463856F6862C2CA66AF6DBE3A84F2DA1C8B507EB1D08AE64E1CA714CD577324EED21B0BC61558683D7BF78D537DB6C01AAFB8D9A6802BBE9BA37117E55838AEC01C8FBF25C702F225E688F2453916552EDD19EB2FFDA671D6AF0A0C0CAC5FDD6C57ADBFBDDCC6A2B45FCD91AACB022C4CF5C9C2BE9477D3384B53D6B059C1D88B69FC22C696EC8C26B7D1C448CFA7DEC81AE0F4689B8ED3EA5792A86AA76E9C6C15BB7AF6523601D9CE2C3285E5567B992C86727BB313A3BDA2C3E1B49F5F5E87F848BB23326136658D0310A68D55A041B711BAB2F7A40D57596C466AD4C0A8F4A938CDAA01061358ED9F0C24D08E2E71DF64EC8037191586835DD49F78A099FDE3A1834C21A61F6066A7FC4506D729F2BFFDA0383B3CCFE89DBEE6768B01BBA37543DC69EFF32CD9BA262EA442E2EA1DF981D22EC0265003F516FF4EA98086D5D12A209C39986C1056554D370115F2561E2CBCB20E28CF4CBE92295E3E3E31F1AA99EAA6DEB36A0AD59D8F0C945D0333C5F4AE3B06DA29D3EEE0786AD5280E942C74A3A83F9D72300746A3B483E24CAF1ECCD1D80EEA879AE77E05914ED7C42A4D34D29CB209A7698BEA64ABFF811AE9A8ABAC42536893C7C0A7C75C97E926FB57D87EB90071700FB332785CBADFCD0E6747C2B336BBF3C4CC3CCB7C2E69A6E39D197E985E20B52688F1FBCA58BBB2672C9322CB1DB5B287F222570B6092C2D22CCF2C91D25DB0F3D8874F4BF7DF45A313E7FC1F9FAB76EF9C2B44B4E4C4D973FE33557E7347F4F2F58E13F79448D9C5A847440641482F870C436123784BB561DA5AA88E9A9341FD97ED747DCB1663F7DAC7FF89B28E9D54D8573386B4171FC41882A178EB6298D9C94F5DDC8709C0C7B604C94F5F0CC3513C8511E711245F3B9EC2B0EB42F534C620E1E91EC53000B37B0EE3CDF08433B6E1185C82EE1080AEC720062E5BF2C30FF48DC6ACDCC0B744DB92C73362E950076C5FAF167301B3A59499B6E692D69150C7648368281B6F61B8FB0EE5BEDE811F3D757069FB8300C48C7CD37942377AA33332F923BC17CB8CE46810926686273112F583882A070857499C610402F974943828B117A420142520EFDA98A835E5AD81144B3EC29460D3A36F15A7261DF69C47361D08D2EE93C4763253592FD4224B749794417DFD711714417F66F9024A60919D3B45326EB97367977F3B74C83577DE261FF2862F93BE3A8EA55E60B4CDF2938D067B4B4B86D5B2F595298AD582F4BA8A629EA33DC5CCD026A60E4A6EDEE941EFB8AB28F7D573C8F9F203DF95A3FE36F2938FBCFE0073F2A157A7E8CB4969E2C8A993EF3B73EFCBD3BEA59B16EFE290A12EA3217DB67467767E7F72BEA6BFEEC466650EBF36855FC71252A53AAB12FCB5F9FD1A6475F2A42EFBBF33F95F271E5DAAA7C1F30086AF03687A563E37F0CA2F09888ACD5F65324E8AD767D6EFE61301ACD2B7F95FBBC1EEC4F9FF526E726FB67F19BCF4B47ACDFC7E7BB6ACD47C1732F8ED5914E744FEAED24EE6E6BF1E931639F7F2ED1FE28D30FF8915F187B260D342D04B4D31F4383FA4A9731EDF27B55324505457112F36400C7CE2A49C221CDC030F93620F6659C0BC30F829BA83FE797C95E334C7846518DD855C0A2775ABBAFA2F1E16E0695E5CA585FB30050B84CC80B000AFE21FF3206C9FEC3B536C136B20A8BF566DA417675E74437DD3BED67999C4864095F81A37F316466948C0B2AB780D1EE110DA88D2FE0A37C07BAEEF74E941FA078217FBE263003608445985D1B6273F890EFBD1D3F7FF136000B81D3F50CB6D0000', 'hex') , '6.1.1-30610');