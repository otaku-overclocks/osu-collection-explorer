<windows:BaseNavigationWindow x:Class="osu_collection_manager.UI.Windows.CEMainWindow"
                              xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                              xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                              xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                              xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                              xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
                              xmlns:windows="clr-namespace:osu_collection_manager.UI.Windows"
                              mc:Ignorable="d"
                              Background="{DynamicResource MaterialDesignPaper}"
                              Title="{Binding ElementName=windowTitle, Path=Text}" Height="430.515" Width="604.123"
                              AllowsTransparency="True"
                              WindowStyle="None"
                              WindowState="Normal"
                              ResizeMode="CanResize"
                              UseLayoutRounding="True"
                              AllowDrop="True" Drop="OnFileDrop"
                              DataContext="{Binding RelativeSource={RelativeSource Self}}">
    <WindowChrome.WindowChrome>
        <WindowChrome
            CaptionHeight="48"
            ResizeBorderThickness="5"
            UseAeroCaptionButtons="True" />
    </WindowChrome.WindowChrome>
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="48" />
            <RowDefinition />
        </Grid.RowDefinitions>
        <materialDesign:DialogHost x:Name="MainDialog" Grid.Row="1" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" Style="{StaticResource DialogStyleDark}">
            <materialDesign:DialogHost.DialogContent >
                <Frame x:Name="ModalContent" NavigationUIVisibility="Hidden" Width="{Binding ModalSize.X}"
                       Height="{Binding ModalSize.Y}" />
            </materialDesign:DialogHost.DialogContent>
            <Frame x:Name="WindowContent" NavigationUIVisibility="Hidden" HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
        </materialDesign:DialogHost>

        <Border Grid.Row="0" Background="{DynamicResource MaterialDesignPaper}">
            <Border.Effect>
                <DropShadowEffect Direction="270" Opacity="0.25" RenderingBias="Quality" BlurRadius="4" ShadowDepth="2" />
            </Border.Effect>
            <DockPanel VerticalAlignment="Top" HorizontalAlignment="Stretch" Margin="8">
                <TextBlock x:Name="windowTitle" Text="osu! collection manager beta 0.1" VerticalAlignment="Center"
                           Foreground="White" Opacity="0.54"
                           FontSize="16" FontWeight="Bold" Margin="8,0,0,0" />
                <Button WindowChrome.IsHitTestVisibleInChrome="True" Style="{StaticResource WindowNavButton}"
                        Content="{StaticResource CloseImage}" HorizontalAlignment="Right" DockPanel.Dock="Right"
                        Click="Close" />
                <Button WindowChrome.IsHitTestVisibleInChrome="True" Style="{StaticResource WindowNavButton}"
                        Content="{StaticResource MaximizeImage}" HorizontalAlignment="Right" DockPanel.Dock="Right"
                        Click="Maximize" />
                <Button WindowChrome.IsHitTestVisibleInChrome="True" Style="{StaticResource WindowNavButton}"
                        Content="{StaticResource MinimizeImage}" HorizontalAlignment="Right" DockPanel.Dock="Right"
                        Click="Minimize" />
                <Button WindowChrome.IsHitTestVisibleInChrome="True" Style="{StaticResource WindowNavButton}"
                        Content="{StaticResource SettingsImage}" HorizontalAlignment="Right" DockPanel.Dock="Right"
                        Click="OpenSettings" />
                <Button WindowChrome.IsHitTestVisibleInChrome="True" Style="{StaticResource WindowNavButton}"
                        HorizontalAlignment="Right" DockPanel.Dock="Right"
                        Click="GoGithub">
                    <Button.Content>
                        <materialDesign:PackIcon Kind="GithubCircle" Height="24" Width="24" Foreground="White" />
                    </Button.Content>
                </Button>

            </DockPanel>
        </Border>

        <Grid x:Name="LoadingOverlay" Grid.Row="1" Background="{StaticResource MaterialDesignPaper}"
              Visibility="Hidden">
            <ProgressBar VerticalAlignment="Center" HorizontalAlignment="Center" Width="54"
                         Style="{DynamicResource MaterialDesignCircularProgressBar}"
                         Foreground="{StaticResource SecondaryAccentBrush}" IsIndeterminate="True" />
        </Grid>
    </Grid>


</windows:BaseNavigationWindow>