# 🥞 Pancake Flip VR

Kleine VR-game gemaakt in Unity 6 waarbij je een pannenkoek maakt en flipt.  
Gebruik je VR-headset of test het voorlopig in de editor met mock inputs.

---

## 🚀 Installatie-instructies

### 1. Vereisten
- Unity Hub + Unity versie **6.0.0 (LTS)**
- Git geïnstalleerd
- (Optioneel) Android Build Support voor Meta Quest

### 2. Project clonen

```bash
git clone https://github.com/atoolate/expdev-pancakeflip.git
cd <projectmap>
```

### 3. Project openen in Unity
- Open **Unity Hub**
- Klik op **"Open"**
- Selecteer de folder die je net hebt gecloned
- Open de scene: `Assets/Scenes/MainScene.unity`

---

## 🛠 Tips

- Zet XR Plugin Management aan als Unity hierom vraagt (`Edit > Project Settings > XR Plugin Management`)
- Test in de editor met Mock HMD als je geen headset hebt
- Gebruik `XR Grab Interactable`, `Rigidbody` en een `Collider` om objecten oppakbaar te maken

---

## 📁 Projectstructuur

```
Assets/
├── Models/        # Alle 3D-modellen (.fbx of .glb)
├── Prefabs/       # Interactables zoals melk, ei, pan, ...
├── Scripts/       # C# scripts (komt nog)
├── Scenes/        # MainScene.unity zit hier
```

---

## 👥 Team

- **Alex** – setup, structuur, imports
- **Teammate 1** – mixing + interaction scripts
- **Teammate 2** – flipping + timing mechanic

---

## ⚠️ Opmerkingen

- Gebruik altijd prefabs voor objecten die je deelt of hergebruikt
- Project is gebouwd met Unity 6.0.0 — gebruik exact die versie
- Test builds later pas op Meta Quest als MVP af is
