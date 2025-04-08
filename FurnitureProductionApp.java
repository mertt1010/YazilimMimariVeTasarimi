import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

// modern tarzda concrete factory 
class ModernFurnitureFactory implements FurnitureFactory {
    @Override
    public Chair createChair() {
        return new ModernChair();
    }

    @Override
    public Sofa createSofa() {
        return new ModernSofa();
    }

    @Override
    public CoffeeTable createCoffeeTable() {
        return new ModernCoffeeTable();
    }
}
// modern tarzında sandalye
class ModernChair implements Chair {
    @Override
    public void sitOn() {
        System.out.println("Sitting on Modern Chair");
    }

    @Override
    public Chair clone() {
        return new ModernChair();
    }
}

// modern tarzında kanepe
class ModernSofa implements Sofa {
    @Override
    public void lieOn() {
        System.out.println("Lying on Modern Sofa");
    }

    @Override
    public Sofa clone() {
        return new ModernSofa();
    }
}

// modern tarzında sehpa
class ModernCoffeeTable implements CoffeeTable {
    @Override
    public void putCoffee() {
        System.out.println("Putting coffee on Modern Coffee Table");
    }

    @Override
    public CoffeeTable clone() {
        return new ModernCoffeeTable();
    }
}
// Victorian tarzında mobilyaları üreten Concrete Factory
class VictorianFurnitureFactory implements FurnitureFactory {
    @Override
    public Chair createChair() {
        return new VictorianChair();
    }

    @Override
    public Sofa createSofa() {
        return new VictorianSofa();
    }

    @Override
    public CoffeeTable createCoffeeTable() {
        return new VictorianCoffeeTable();
    }
}

// Victorian tarzında sandalye
class VictorianChair implements Chair {
    @Override
    public void sitOn() {
        System.out.println("Sitting on Victorian Chair");
    }

    @Override
    public Chair clone() {
        return new VictorianChair();
    }
}

// Victorian tarzında kanepe
class VictorianSofa implements Sofa {
    @Override
    public void lieOn() {
        System.out.println("Lying on Victorian Sofa");
    }

    @Override
    public Sofa clone() {
        return new VictorianSofa();
    }
}

// Victorian tarzında sehpa
class VictorianCoffeeTable implements CoffeeTable {
    @Override
    public void putCoffee() {
        System.out.println("Putting coffee on Victorian Coffee Table");
    }

    @Override
    public CoffeeTable clone() {
        return new VictorianCoffeeTable();
    }
}


// Mobilya üretimi için Abstract Factory arayüzü
interface FurnitureFactory {
    Chair createChair();
    Sofa createSofa();
    CoffeeTable createCoffeeTable();
}

// Art Deco tarzında mobilyaları üreten Concrete Factory
class ArtDecoFurnitureFactory implements FurnitureFactory {
    @Override
    public Chair createChair() {
        return new ArtDecoChair();
    }

    @Override
    public Sofa createSofa() {
        return new ArtDecoSofa();
    }

    @Override
    public CoffeeTable createCoffeeTable() {
        return new ArtDecoCoffeeTable();
    }
}

// Art Deco tarzında sandalye
class ArtDecoChair implements Chair {
    @Override
    public void sitOn() {
        System.out.println("Sitting on Art Deco Chair");
    }

    @Override
    public Chair clone() {
        return new ArtDecoChair(); // Prototype desenini kullanarak klonlama
    }
}

// Art Deco tarzında kanepe
class ArtDecoSofa implements Sofa {
    @Override
    public void lieOn() {
        System.out.println("Lying on Art Deco Sofa");
    }

    @Override
    public Sofa clone() {
        return new ArtDecoSofa(); // Prototype desenini kullanarak klonlama
    }
}

// Art Deco tarzında sehpa
class ArtDecoCoffeeTable implements CoffeeTable {
    @Override
    public void putCoffee() {
        System.out.println("Putting coffee on Art Deco Coffee Table");
    }

    @Override
    public CoffeeTable clone() {
        return new ArtDecoCoffeeTable(); // Prototype desenini kullanarak klonlama
    }
}

// Sandalye arayüzü
interface Chair {
    void sitOn();
    Chair clone();
}

// Kanepe arayüzü
interface Sofa {
    void lieOn();
    Sofa clone();
}

// Sehpa arayüzü
interface CoffeeTable {
    void putCoffee();
    CoffeeTable clone();
}

// Singleton deseni kullanarak fabrika 
class FurnitureFactorySingleton {
    private static FurnitureFactory instance;

    private FurnitureFactorySingleton() {}

    public static FurnitureFactory getInstance() {
        if (instance == null) {
            instance = new ArtDecoFurnitureFactory(); // Varsayılan olarak Art Deco fabrikası
        }
        return instance;
    }
}

// GUI
public class FurnitureProductionApp extends JFrame {
    private FurnitureFactory furnitureFactory;
    private JTextArea logTextArea;
    private JPanel imagePanel; // Resim paneli

    public FurnitureProductionApp() {
        setTitle("Furniture Production App");
        setSize(1000, 1000); // Pencere boyutunu genişlet
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        chooseFurnitureFactory(); // Mobilya tarzını seçme

        JPanel mainPanel = new JPanel(new BorderLayout());

        JButton createChairButton = new JButton("Create Chair");
        createChairButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                Chair chair = furnitureFactory.createChair();
                logTextArea.append("Chair created: " + chair.getClass().getSimpleName() + "\n");
                showFurnitureImage("artchair.jpg"); //RESİMLERİ EKLEMEDE KALDIM
            }
        });

        JButton createSofaButton = new JButton("Create Sofa");
        createSofaButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                Sofa sofa = furnitureFactory.createSofa();
                logTextArea.append("Sofa created: " + sofa.getClass().getSimpleName() + "\n");
                showFurnitureImage("art_sofa.jpg"); //resmi gösterme
            }
        });

        JButton createCoffeeTableButton = new JButton("Create Coffee Table");
        createCoffeeTableButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                CoffeeTable coffeeTable = furnitureFactory.createCoffeeTable();
                logTextArea.append("Coffee Table created: " + coffeeTable.getClass().getSimpleName() + "\n");
                showFurnitureImage( "art_coffer_table.jpg"); // Sehpa resmini göster
            }
        });

        logTextArea = new JTextArea();
        JScrollPane scrollPane = new JScrollPane(logTextArea);

        // Resim panelini oluştur
        imagePanel = new JPanel();
        imagePanel.setPreferredSize(new Dimension(200, 200)); // Resim paneli boyutunu ayarla

        JPanel buttonPanel = new JPanel();
        buttonPanel.setLayout(new GridLayout(3, 1));
        buttonPanel.add(createChairButton);
        buttonPanel.add(createSofaButton);
        buttonPanel.add(createCoffeeTableButton);

        mainPanel.add(buttonPanel, BorderLayout.WEST);
        mainPanel.add(scrollPane, BorderLayout.CENTER);
        mainPanel.add(imagePanel, BorderLayout.EAST); // Resim panelini ana panele ekledik east west center ile

        add(mainPanel);
        setVisible(true);
    }

    private void chooseFurnitureFactory() {
        // Kullanıcıya hangi mobilya tarzını seçmek istediğini sorma
        String[] options = {"Art Deco", "Victorian", "Modern"};
        int choice = JOptionPane.showOptionDialog(null, "Choose furniture style:", "Furniture Style", JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, null, options, options[0]);
    
        // Kullanıcının seçimine göre ilgili mobilya fabrikasını oluşturduk
        switch (choice) {
            case 0:
                furnitureFactory = new ArtDecoFurnitureFactory();
                break;
            case 1:
                furnitureFactory = new VictorianFurnitureFactory();
                break;
            case 2:
                furnitureFactory = new ModernFurnitureFactory();
                break;
            default:
                System.exit(0); // Uygulamayı kapatıyoruz
        }
    }
    

    // Resim panelinde mobilya resmini göstermek için
    private void showFurnitureImage(String imageName) {
        

        ImageIcon imageIcon = new ImageIcon(imageName); 
        JLabel imageLabel = new JLabel(imageIcon);
        imagePanel.removeAll(); //resim kısmını sıfırla
        imagePanel.add(imageLabel, BorderLayout.EAST);
        imagePanel.revalidate();// Yeniden düzenle
        imagePanel.repaint();// Yeniden boyama
    }



    public static void main(String[] args) {
        new FurnitureProductionApp();
    }
}
