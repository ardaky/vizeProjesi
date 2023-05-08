using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vizeProjesi.Models;
using vizeProjesi.ViewModel;

namespace vizeProjesi.Controllers
{
    public class ServisController : ApiController
    {
        DatabaseProjeVizeEntities db = new DatabaseProjeVizeEntities();
        SonucModel sonuc = new SonucModel();

        #region Egitim

        // Tüm eğitimleri listeler.
        [HttpGet]
        [Route("api/egitimlistele")]
        public List<EgitimModel> egitimListele()
        {
            List<EgitimModel> liste = db.Egitim.Select(x => new EgitimModel()
            {

                egitimId = x.egitimId,
                egitimAdi = x.egitimAdi,
                egitimAciklamasi = x.egitimAciklamasi,
                egitimFoto = x.egitimFoto,
                egitimiVerenId = x.egitimiVerenId,
                egitimKatId = x.egitimKatId,
                egitimKatAdi = x.Kategori.katAdi,
                egitimUcretliMi = x.egitimUcretliMi

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimiVerenKullanici = kullaniciById(kayit.egitimiVerenId);
            }

            return liste;
        }

        // ID numarasına göre eğitimleri listeler.
        [HttpGet]
        [Route("api/egitimbyid/{egitimId}")]
        public EgitimModel egitimById(int egitimId)
        {
            EgitimModel kayit = db.Egitim.Where(s => s.egitimId == egitimId).Select(x => new EgitimModel()
            {
                egitimId = x.egitimId,
                egitimAdi = x.egitimAdi,
                egitimAciklamasi = x.egitimAciklamasi,
                egitimFoto = x.egitimFoto,
                egitimiVerenId = x.egitimiVerenId,
                egitimKatId = x.egitimKatId,
                egitimKatAdi = x.Kategori.katAdi,
                egitimUcretliMi = x.egitimUcretliMi

            }).SingleOrDefault();

            if(kayit != null)
            {
                kayit.egitimiVerenKullanici = kullaniciById(kayit.egitimiVerenId);
            }
           
            return kayit;
        }

        // Kategori ID'sine göre eğitimleri listeler.
        [HttpGet]
        [Route("api/egitimbykatid/{egitimKatId}")]
        public List<EgitimModel> egitimByKatId(int egitimKatId)
        {
            List<EgitimModel> liste = db.Egitim.Where(s=> s.egitimKatId == egitimKatId).Select(x => new EgitimModel()
            {

                egitimId = x.egitimId,
                egitimAdi = x.egitimAdi,
                egitimAciklamasi = x.egitimAciklamasi,
                egitimFoto = x.egitimFoto,
                egitimiVerenId = x.egitimiVerenId,
                egitimKatId = x.egitimKatId,
                egitimKatAdi = x.Kategori.katAdi,
                egitimUcretliMi = x.egitimUcretliMi

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimiVerenKullanici = kullaniciById(kayit.egitimiVerenId);
            }

            return liste;
        }

        [HttpGet]
        [Route("api/egitimByEgitmenId/{egitmenId}")]
        public List<EgitimModel> egitimByEgitmenId(int egitmenId)
        {
            List<EgitimModel> liste = db.Egitim.Where(s => s.egitimiVerenId == egitmenId).Select(x => new EgitimModel()
            {

                egitimId = x.egitimId,
                egitimAdi = x.egitimAdi,
                egitimAciklamasi = x.egitimAciklamasi,
                egitimFoto = x.egitimFoto,
                egitimiVerenId = x.egitimiVerenId,
                egitimKatId = x.egitimKatId,
                egitimKatAdi = x.Kategori.katAdi,
                egitimUcretliMi = x.egitimUcretliMi

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimiVerenKullanici = kullaniciById(kayit.egitimiVerenId);
            }

            return liste;
        }

        // Yeni eğitim ekler.
        [HttpPost]
        [Route("api/egitimekle")]
        public SonucModel egitimEkle(EgitimModel model)
        {   
            if(db.Egitim.Count(s=> s.egitimAdi == model.egitimAdi && s.egitimiVerenId == model.egitimiVerenId && s.egitimKatId == model.egitimKatId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Zaten Olan Bir Eğitim Tekrar Eklenemez !";
                return sonuc;
            }
            
            Egitim yeni = new Egitim();
            yeni.egitimAdi = model.egitimAdi;
            yeni.egitimFoto = model.egitimFoto;
            yeni.egitimiVerenId = model.egitimiVerenId;
            yeni.egitimKatId = model.egitimKatId;
            yeni.egitimUcretliMi = model.egitimUcretliMi;
            yeni.egitimAciklamasi = model.egitimAciklamasi;
            db.Egitim.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Başarıyla Eklendi !";
            
            return sonuc;
        }


        // Eğitimi Düzenler
        [HttpPut]
        [Route("api/egitimduzenle")]
        public SonucModel egitimDuzenle(EgitimModel model)
        {
            Egitim kayit = db.Egitim.Where(s => s.egitimId == model.egitimId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Eğitim Bulunamadı !";
                return sonuc;
            }

            kayit.egitimAdi = model.egitimAdi;
            kayit.egitimiVerenId = model.egitimiVerenId;
            kayit.egitimFoto = model.egitimFoto;
            kayit.egitimAciklamasi = model.egitimAciklamasi;
            kayit.egitimUcretliMi = model.egitimUcretliMi;
            kayit.egitimKatId = model.egitimKatId;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Başarıyla Düzenlendi !";

            return sonuc;
        }


        // Eğitimi siler
        [HttpDelete]
        [Route("api/egitimsil/{egitimId}")]
        public SonucModel egitimSil(int egitimId)
        {
            Egitim kayit = db.Egitim.Where(s => s.egitimId == egitimId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Eğitim Bulunamadı !";
                return sonuc;
            }

            if(db.Kayit.Count(s=> s.kayitEgitimId == egitimId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde öğrenci kaydı olan eğitim silinemez !";
                return sonuc;
            }

            db.Egitim.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Başarıyla Silindi !";
            return sonuc;
        }

        #endregion

        #region Kullanici

        [HttpGet]
        [Route("api/kullanicilistele")]
        public List<KullaniciModel> kullaniciListele()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            {

                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                sifre = x.sifre,
                foto = x.foto,
                rol = x.rol,


            }).ToList();
            

            return liste;
        }

        [HttpGet]
        [Route("api/kullanicibyid/{kullaniciId}")]
        public KullaniciModel kullaniciById(int kullaniciId)
        {
            KullaniciModel kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdi = x.kullaniciAdi,
                adSoyad = x.adSoyad,
                email = x.email,
                sifre = x.sifre,
                foto = x.foto,
                rol = x.rol
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/kullaniciekle")]
        public SonucModel kullaniciEkle(KullaniciModel model)
        {
            if(db.Kullanici.Count(s=> s.email == model.email || s.kullaniciAdi == model.kullaniciAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girmiş olduğunuz e-posta ya da kullanıcı adı zaten kayıtlıdır !";
                return sonuc;
            }

            Kullanici yeni = new Kullanici();
            yeni.adSoyad = model.adSoyad;
            yeni.email = model.email;
            yeni.sifre = model.sifre;
            yeni.foto = model.foto;
            yeni.kullaniciAdi = model.kullaniciAdi;
            yeni.rol = model.rol;
            db.Kullanici.Add(yeni);
            db.SaveChanges();
            
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Kaydı Başarıyla Yapıldı !";
            return sonuc;
        }

        // Kullanıcı bilgilerini günceller. 
        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public SonucModel kullaniciDuzenle(KullaniciModel model)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == model.kullaniciId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            if(db.Kullanici.Count(s=> s.email == model.email) > 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu mail adresi zaten bir üyeye ait.";
                return sonuc;
            }
            
            if (db.Kullanici.Count(s => s.kullaniciAdi == model.kullaniciAdi) > 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu kullanıcı adı zaten bir üyeye ait.";
                return sonuc;
            }


            kayit.adSoyad = model.adSoyad;
            kayit.email = model.email;
            kayit.sifre = model.sifre;
            kayit.rol = model.rol;
            kayit.foto = model.foto;
            kayit.kullaniciAdi = model.kullaniciAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Başarıyla Güncellendi !";
            
            return sonuc;
        }

        //Kullanıcının kaydını siler.
        [HttpDelete]
        [Route("api/kullanicisil/{kullaniciId}")]
        public SonucModel kullaniciSil(int kullaniciId)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            if(db.Egitim.Count(s=> s.egitimiVerenId == kullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Eğitim veren eğitmenlerin kaydının silinebilmesi için verdiği eğitimleri devretmesi veya kapatması gerekir !";
                return sonuc;
            }

            List<YorumModel> yorumlar = yorumByKullanici(kullaniciId);
            foreach (var i in yorumlar)
            {
                yorumSil(i.yorumId);
            }

            List<KayitModel> kayitlar = kullaniciEgitimListeleKayit(kullaniciId);
            foreach (var i in kayitlar)
            {
                kayitSil(i.kayitId);
            }

            db.Kullanici.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcıya ait tüm yorumlar, eğitim kayıtları ve bilgiler silinmiştir.";

            return sonuc;
        }

        #endregion

        #region Kategori

        // Kategorileri listeler.
        [HttpGet]
        [Route("api/kategorilistele")]
        public List<KategoriModel> kategoriListele()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                katId = x.katId,
                katAdi = x.katAdi
            }).ToList();

            return liste;
        }

        // Gelen kategori ıd numarasına ait kategori kaydını döndürür.
        [HttpGet]
        [Route("api/kategoriById/{katId}")]
        public KategoriModel kategoriById(int katId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.katId == katId).Select(x => new KategoriModel()
            {
                katId = x.katId,
                katAdi = x.katAdi

            }).SingleOrDefault();

            return kayit;
        }

        // Kendisine gelen kategori model ile yeni bir kategori ekler.
        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel kategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.katAdi == model.katAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Kategori Adı Zaten Kayıtlı !";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.katAdi = model.katAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi !";

            return sonuc;
        }

        // Kendisine gelen kategoriyi düzenler.
        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel kategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == model.katId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı !";
                return sonuc;
            }

            kayit.katAdi = model.katAdi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi !";
            return sonuc;
        }

        // Kendisine gelen kategori ıd'sine sahip kategoriyi üzerinde kayıt yoksa siler.
        [HttpDelete]
        [Route("api/kategorisil/{katId}")]
        public SonucModel kategoriSil(int katId)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == katId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı !";
                return sonuc;
            }

            if (db.Egitim.Count(s => s.egitimKatId == katId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Eğitim Kaydı Olan Kategori Silinemez !";
                return sonuc;
            }

            db.Kategori.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Başarıyla Silindi !";
            return sonuc;
        }

        #endregion

        #region Kayit


        // Tüm kayıtları listeler.
        [HttpGet]
        [Route("api/kayitlistele")]
        public List<KayitModel> kayitListele()
        {
            List<KayitModel> liste = db.Kayit.Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitEgitimId = x.kayitEgitimId,
                kayitKullaniciId = x.kayitKullaniciId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimBilgi = egitimById(kayit.kayitEgitimId);
                kayit.kullaniciBilgi = kullaniciById(kayit.kayitKullaniciId);
            }

            return liste;
        }

        //  Kullanıcının aldığı/verdiği eğitimleri listeliyoruz.
        // Eğitimleri içeren liste döndürür. (EĞİTİMODEL)
        [HttpGet]
        [Route("api/kullaniciegitimlistele/{kullaniciId}")]
        public List<EgitimModel> kullaniciEgitimListele(int kullaniciId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitKullaniciId == kullaniciId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitEgitimId = x.kayitEgitimId,
                kayitKullaniciId = x.kayitKullaniciId
            }).ToList();

            var egitimler= new List<EgitimModel>();

            foreach (var kayit in liste)
            {
                kayit.egitimBilgi = egitimById(kayit.kayitEgitimId);
                kayit.kullaniciBilgi = kullaniciById(kayit.kayitKullaniciId);
                if(kayit.kayitEgitimId != null){
                    egitimler.Add(egitimById(kayit.kayitEgitimId));
                }
                
            }

            return egitimler;
        }

        // Kullanıcının aldığı/verdiği eğitimleri listeler.
        // Kayıtlardan oluşan liste döndürür. (KAYİTMODEL)
        [HttpGet]
        [Route("api/kullaniciegitimlistelek/{kullaniciId}")]
        public List<KayitModel> kullaniciEgitimListeleKayit(int kullaniciId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitKullaniciId == kullaniciId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitEgitimId = x.kayitEgitimId,
                kayitKullaniciId = x.kayitKullaniciId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimBilgi = egitimById(kayit.kayitEgitimId);
                kayit.kullaniciBilgi = kullaniciById(kayit.kayitKullaniciId);
            }

            return liste;
        }


        // Eğitimi Alan kullanıcıları listeliyoruz. 
        // Kullanıcı kayıtlarından oluşan liste döndürür. (KULLANİCİMODEL)
        [HttpGet]
        [Route("api/egitimkullanicilistele/{egitimId}")]
        public List<KullaniciModel> egitimKullaniciListele(int egitimId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitEgitimId == egitimId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitEgitimId = x.kayitEgitimId,
                kayitKullaniciId = x.kayitKullaniciId
            }).ToList();

            var kullanicilar = new List<KullaniciModel>();

            foreach (var kayit in liste)
            {
                kayit.egitimBilgi = egitimById(kayit.kayitEgitimId);
                kayit.kullaniciBilgi = kullaniciById(kayit.kayitKullaniciId);
                kullanicilar.Add(kullaniciById(kayit.kayitKullaniciId));
            }

            return kullanicilar;
        }


        // Eğitimi Alan kullanıcıları listeliyoruz. 
        // Kayıtlardan oluşan liste döndürür. (KAYİTMODEL)
        [HttpGet]
        [Route("api/egitimkullanicilistelek/{egitimId}")]
        public List<KayitModel> egitimKullaniciListeleKayit(int egitimId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitEgitimId == egitimId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitEgitimId = x.kayitEgitimId,
                kayitKullaniciId = x.kayitKullaniciId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.egitimBilgi = egitimById(kayit.kayitEgitimId);
                kayit.kullaniciBilgi = kullaniciById(kayit.kayitKullaniciId);          
            }
            
            return liste;
        }


        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel kayitEkle(KayitModel model)
        {
            if(db.Kayit.Count(s=> s.kayitEgitimId == model.kayitEgitimId && s.kayitKullaniciId == model.kayitKullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Kullanıcı Zaten Bu Eğitimi Alıyor !";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitEgitimId = model.kayitEgitimId;
            yeni.kayitKullaniciId = model.kayitKullaniciId;
            db.Kayit.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Eklendi !";
            
            return sonuc;
        }

        // Kullanıcının eğitim Kaydını Siler
        [HttpDelete]
        [Route("api/kayitSil")]
        public SonucModel kayitSil(int kayitId)
        {
            

            Kayit skayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if(skayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }
            db.Kayit.Remove(skayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Kaydı Başarıyla Silindi !";
            return sonuc;

        }

        #endregion

        #region Yorum

        // Tüm yorumları listeler.
        [HttpGet]
        [Route("api/yorumlistele")]
        public List<YorumModel> yorumListele()
        {
            List<YorumModel> liste = db.Yorum.Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumYapanId = x.yorumYapanId,
                yorumYapilanEgitimId = x.yorumYapilanEgitimId,
                yorumİcerigi = x.yorumİcerigi,
                yorumYapilanEgitimAdi = x.Egitim.egitimAdi
            
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumYapanKullanici = kullaniciById(kayit.yorumYapanId);                               
            }

            return liste;
        }

        // ID numarasına göre yorum getirir.
        [HttpGet]
        [Route("api/yorumlistelebyid/{yorumId}")]
        public YorumModel yorumListele(int yorumId)
        {
            YorumModel kayit = db.Yorum.Where(s => s.yorumId == yorumId).Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumYapanId = x.yorumYapanId,
                yorumYapilanEgitimId = x.yorumYapilanEgitimId,
                yorumİcerigi = x.yorumİcerigi,
                yorumYapilanEgitimAdi = x.Egitim.egitimAdi

            }).FirstOrDefault();

            if(kayit != null)
            {
                kayit.yorumYapanKullanici = kullaniciById(kayit.yorumYapanId);
            }

            return kayit;
        }

        // Bir kullanıcıya ait yorumları listeleme
        [HttpGet]
        [Route("api/yorumbykullanici/{kullaniciId}")]
        public List<YorumModel> yorumByKullanici(int kullaniciId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.yorumYapanId == kullaniciId).Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumYapanId = x.yorumYapanId,
                yorumYapilanEgitimId = x.yorumYapilanEgitimId,
                yorumİcerigi = x.yorumİcerigi,
                yorumYapilanEgitimAdi = x.Egitim.egitimAdi
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumYapanKullanici = kullaniciById(kayit.yorumYapanId);
            }

            return liste;
        }

        // Bir eğitime ait yorumları listeleme
        [HttpGet]
        [Route("api/yorumbyegitim/{egitimId}")]
        public List<YorumModel> yorumByEgitim(int egitimId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.yorumYapilanEgitimId == egitimId).Select(x => new YorumModel()
            {
                yorumId = x.yorumId,
                yorumYapanId = x.yorumYapanId,
                yorumYapilanEgitimId = x.yorumYapilanEgitimId,
                yorumİcerigi = x.yorumİcerigi,
                yorumYapilanEgitimAdi = x.Egitim.egitimAdi
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yorumYapanKullanici = kullaniciById(kayit.yorumYapanId);
            }

            return liste;
        }

        // Yorum Ekler.
        [HttpPost]
        [Route("api/yorumekle")]
        public SonucModel yorumEkle(YorumModel model)
        {
            if(db.Yorum.Count(s=> s.yorumİcerigi == model.yorumİcerigi && s.yorumYapilanEgitimId == model.yorumYapilanEgitimId && s.yorumYapanId == model.yorumYapanId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu yorum zaten bu kullanıcı tarafından bu eğitime daha önce yapılmış !";
                return sonuc;
            }

            Yorum yeni = new Yorum();
            yeni.yorumYapanId = model.yorumYapanId;
            yeni.yorumYapilanEgitimId = model.yorumYapilanEgitimId;
            yeni.yorumİcerigi = model.yorumİcerigi;
            db.Yorum.Add(yeni);
            db.SaveChanges();       

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Eklendi !";
            return sonuc;
        }

        // Yorumu siler.
        [HttpDelete]
        [Route("api/yorumsil/{yorumId}")]
        public SonucModel yorumSil(int yorumId)
        {
            Yorum kayit = db.Yorum.Where(s => s.yorumId == yorumId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı !";
                return sonuc;
            }

            db.Yorum.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Silindi !";
            return sonuc;
        }
        #endregion
    }
}
